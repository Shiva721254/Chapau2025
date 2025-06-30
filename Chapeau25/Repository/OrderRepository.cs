using Chapeau25.Enums;
using Chapeau25.Models;
using Chapeau25.Service;
using Chapeau25.ViewModel;
using Microsoft.Data.SqlClient;
using static NuGet.Packaging.PackagingConstants;

namespace Chapeau25.Repositories
{
    public class OrderRepository() : IOrderRepository
    {
        public List<BarAndKitchenViewModel> GetOrders(OrderFilter filter)
        {
            var orders = new List<BarAndKitchenViewModel>();

            var connection = ExtentionMethods.DatabaseHelper.GetConnection();

            var (query, parameters) = BuildSimpleQuery(filter);

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddRange(parameters);
            connection.Open();

            int lastOrderId = -1;
            BarAndKitchenViewModel? currentOrder = null;

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                int currentOrderId = (int)reader["OrderId"]; 

                // If it's a new order, create a new Order object and add to the list
                if (currentOrderId != lastOrderId) 
                { 
                    currentOrder = ReadOrder(reader);
                    orders.Add(currentOrder);
                    lastOrderId = currentOrderId;
                }

                // Always add the current item to the latest order
                currentOrder?.OrderItems.Add(ReadOrderItem(reader));
            }

            return orders;
        }

        private (string query, SqlParameter[] parameters) BuildSimpleQuery(OrderFilter filter)
        {
            bool showServed = filter == OrderFilter.KitchenServed || filter == OrderFilter.BarServed;
            bool showDrinks = filter == OrderFilter.BarCurrent || filter == OrderFilter.BarServed;

            // SQL query template
            string query = @"
                SELECT 
                    oi.orderItem_id AS OrderItemID,
                    o.order_id AS OrderId,
                    CONCAT(e.first_name, ' ', e.last_name) AS EmployeeName,
                    oi.OrderItemStatus AS OrderStatus,
                    t.table_number AS TableNumber,
                    o.started_time AS OrderedTime,
                    m.name AS ItemName,
                    m.price AS ItemPrice,
                    oi.comment,
                    oi.Quantity,
                    m.type
                FROM Orders_old o
                JOIN [Table] t ON o.order_id = t.table_id
                JOIN Employee e ON o.employee_id = e.employee_id
                JOIN ORDER_ITEM oi ON o.order_id = oi.order_id
                JOIN MENU_ITEM m ON oi.menuitem_id = m.menuitem_id
                WHERE oi.OrderItemStatus " + (showServed ? "= @status" : "<> @status") +
                " AND m.type " + (showDrinks ? "= @type" : "<> @type") +
                " ORDER BY o.started_time;";

            // Use parameters to prevent SQL injection
            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@status", "Served"),
                new SqlParameter("@type", "Drink")
            };

            return (query, parameters);
        }

        private BarAndKitchenViewModel ReadOrder(SqlDataReader reader)
        {
            return new BarAndKitchenViewModel(
                (int)reader["OrderId"],
                (string)reader["EmployeeName"],
                (int)reader["TableNumber"],
                (DateTime)reader["OrderedTime"],
                new List<OrderItem>()
            );
        }

        private OrderItem ReadOrderItem(SqlDataReader reader)
        {
            return new OrderItem(
                (int)reader["OrderItemID"],
                (string)reader["ItemName"],
                (decimal)reader["ItemPrice"],
                (int)reader["Quantity"],
                reader["OrderStatus"] == DBNull.Value ? OrderItemStatus.Ordered : (OrderItemStatus)Enum.Parse(typeof(OrderItemStatus), reader["OrderStatus"].ToString()),
                (string)reader["type"],
                reader["comment"] != DBNull.Value ? reader["comment"].ToString() : ""
            );
        }

        public void ChangeOrderItemStatus(int orderItemId, OrderItemStatus orderItemStatus)
        {
            using (var conn = ExtentionMethods.DatabaseHelper.GetConnection())
            {
                string query = "UPDATE ORDER_ITEM SET OrderItemStatus = @OrderItemStatus " +
                               "WHERE orderItem_id = @orderItemId";  

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@OrderItemStatus", orderItemStatus.ToString());
                cmd.Parameters.AddWithValue("@orderItemId", orderItemId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void ChangeKitchenCourseStatus(int orderId, string courseType, OrderItemStatus status)
        {
            using var connection = ExtentionMethods.DatabaseHelper.GetConnection();

            string query = @"
                            UPDATE ORDER_ITEM
                            SET OrderItemStatus = @status
                            FROM ORDER_ITEM
                            JOIN MENU_ITEM ON ORDER_ITEM.menuitem_id = MENU_ITEM.menuitem_id
                            WHERE ORDER_ITEM.order_id = @orderId AND MENU_ITEM.type = @courseType;
                        ";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@status", status.ToString());
            command.Parameters.AddWithValue("@orderId", orderId);
            command.Parameters.AddWithValue("@courseType", courseType);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public void ChangeEntireOrderStatusByType(int orderId,  bool isDrink, OrderItemStatus status)
        {
            using (var connection = ExtentionMethods.DatabaseHelper.GetConnection())
            {
                string query = @"UPDATE ORDER_ITEM
                                SET OrderItemStatus = @status
                                FROM ORDER_ITEM
                                JOIN MENU_ITEM ON ORDER_ITEM.menuitem_id = MENU_ITEM.menuitem_id
                                WHERE ORDER_ITEM.order_id = @orderId AND MENU_ITEM.type " + (isDrink ? "= 'Drink'" : "IN ('Starter', 'Main', 'Dessert')") + ";";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@status", status.ToString());
                    command.Parameters.AddWithValue("@orderId", orderId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}