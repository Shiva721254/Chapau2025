using Chapeau25.Enums;
using Chapeau25.Models;
using Chapeau25.Service;
using Microsoft.Data.SqlClient;
using static NuGet.Packaging.PackagingConstants;

namespace Chapeau25.Repositories
{
    public class KitchenAndBarRepositories : IKitchenAndBarRepositories
    {

        private readonly string? _connectionString;
        public KitchenAndBarRepositories(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("chapeau2025Database");
        }
        public List<Order> GetOrders(OrderFetchFilter filter)
        {
            var orders = new List<Order>();

            using var connection = new SqlConnection(_connectionString);

            // Get the query and parameters based on the selected filter (e.g., kitchen vs bar, served vs not)
            var (query, parameters) = BuildSimpleQuery(filter);

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddRange(parameters);
            connection.Open();

            int lastOrderId = -1;
            Order? currentOrder = null;

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


        private (string query, SqlParameter[] parameters) BuildSimpleQuery(OrderFetchFilter filter)
        {
            bool showServed = filter == OrderFetchFilter.KitchenServed || filter == OrderFetchFilter.BarServed;
            bool showDrinks = filter == OrderFetchFilter.BarCurrent || filter == OrderFetchFilter.BarServed;

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
                FROM Orders o
                JOIN [Table] t ON o.table_id = t.table_id
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

        private Order ReadOrder(SqlDataReader reader)
        {
            int OrderId = (int)reader["OrderId"];
            string EmployeeName = (string)reader["EmployeeName"];
            OrderItemStatus OrderStatus = Enum.Parse<OrderItemStatus>(reader["OrderStatus"].ToString());
            int TableNumber = (int)reader["TableNumber"];
            DateTime OrderdTime = (DateTime)reader["OrderedTime"]; 
            Order order = new Order(OrderId, EmployeeName, OrderStatus, TableNumber, OrderdTime,new List<OrderItem>());
          
            return order;
            
        }

        private OrderItem ReadOrderItem(SqlDataReader reader)
        {
            int OrderItemID = (int)reader["OrderItemID"];
            string ItemName = (string)reader["ItemName"];
            string type = (string)reader["type"];
        //    string comment = (string)reader["comment"];
            decimal ItemPrice = (decimal)reader["ItemPrice"];
            int Quantity = (int)reader["Quantity"];

            OrderItemStatus orderItemStatus = reader["OrderStatus"] == DBNull.Value ? OrderItemStatus.Ordered : (OrderItemStatus)Enum.Parse(typeof(OrderItemStatus), reader["OrderStatus"].ToString());




            return new OrderItem(OrderItemID, ItemName, ItemPrice, Quantity, orderItemStatus, type);
        }
        public void ChangeOrderItemStatus(int orderItemId, OrderItemStatus orderItemStatus)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
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
            using var connection = new SqlConnection(_connectionString);

            string query = @"
                            UPDATE ORDER_ITEM
                            SET OrderItemStatus = @status
                            FROM ORDER_ITEM
                            JOIN MENU_ITEM ON ORDER_ITEM.menuitem_id = MENU_ITEM.menuitem_id
                            WHERE ORDER_ITEM.order_id = @orderId AND MENU_ITEM.type = @courseType;
                        ";

                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@status", status.ToString());     // e.g., "Preparing"
                command.Parameters.AddWithValue("@orderId", orderId);              // e.g., 101
                command.Parameters.AddWithValue("@courseType", courseType);        // e.g., "Main"

          
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

        }

        public void ChangeEntireOrderStatusByType(int orderId,  bool isDrink,OrderItemStatus status)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE ORDER_ITEM
                                SET OrderItemStatus = @status
                                FROM ORDER_ITEM
                                JOIN MENU_ITEM ON ORDER_ITEM.menuitem_id = MENU_ITEM.menuitem_id
                                WHERE ORDER_ITEM.order_id = @orderId AND MENU_ITEM.type " + (isDrink ? "= 'Drink'" : "IN ('Starter', 'Main', 'Dessert')") + ";";

                using (SqlCommand command = new SqlCommand(query, connection))
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