using Chapeau25.Enums;
using Chapeau25.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

public class TableRepository() : ITableRepository
{
    public IEnumerable<TableInfo> GetAllTables()
    {
        var tables = new List<TableInfo>();
        using (var conn = Chapeau25.ExtentionMethods.DatabaseHelper.GetConnection())
        {
            conn.Open();
            var cmd = new SqlCommand("SELECT table_id, table_number, status, capacity FROM [Table]", conn);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    tables.Add(new TableInfo
                    {
                        TableId = reader.GetInt32(0),
                        TableNumber = reader.GetInt32(1),
                        Status = reader.GetString(2),
                        Capacity = reader.GetInt32(3)
                    });
                }
            }
        }
        return tables;
    }

    public IEnumerable<TableOrderStatusViewModel> GetTableOrders()
    {
        var tableDict = new Dictionary<int, TableOrderStatusViewModel>();

        using (var conn = Chapeau25.ExtentionMethods.DatabaseHelper.GetConnection())
        {
            conn.Open();
            var cmd = new SqlCommand(@"
                SELECT t.table_id, t.table_number, o.order_id, oi.OrderItemStatus, mi.name
                FROM [Table] t
                JOIN orders o ON t.table_id = o.table_id
                JOIN ORDER_ITEM oi ON o.order_id = oi.order_id
                JOIN MENU_ITEM mi ON oi.menuitem_id = mi.menuitem_id
            ", conn);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int tableId = reader.GetInt32(0);
                    int tableNumber = reader.GetInt32(1);
                    int orderId = reader.GetInt32(2);
                    string statusStr = reader.GetString(3);
                    string name = reader.GetString(4) ?? "Some food";

                    if (!tableDict.TryGetValue(tableId, out var tableVm))
                    {
                        tableVm = new TableOrderStatusViewModel
                        {
                            TableId = tableId,
                            TableNumber = tableNumber,
                            Orders = new List<OrderStatusInfo>()
                        };
                        tableDict[tableId] = tableVm;
                    }

                    if (!Enum.TryParse<OrderItemStatus>(statusStr.Replace("-", ""), true, out var status))
                        status = OrderItemStatus.Ordered;

                    tableVm.Orders.Add(new OrderStatusInfo
                    {
                        OrderId = orderId,
                        Status = status,
                        Name = name
                    });
                }
            }
        }

        return tableDict.Values;
    }

    public bool HasUnfinishedOrders(int tableId)
    {
        using (var conn = Chapeau25.ExtentionMethods.DatabaseHelper.GetConnection())
        {
            conn.Open();
            var cmd = new SqlCommand(@"
                SELECT COUNT(*) FROM orders o
                JOIN ORDER_ITEM oi ON o.order_id = oi.order_id
                WHERE o.table_id = @tableId AND oi.OrderItemStatus <> 'Served'
            ", conn);
            cmd.Parameters.AddWithValue("@tableId", tableId);
            int unfinishedOrders = (int)cmd.ExecuteScalar();
            return unfinishedOrders > 0;
        }
    }

    public void UpdateTableStatus(int tableId, string newStatus)
    {
        using (var conn = Chapeau25.ExtentionMethods.DatabaseHelper.GetConnection())
        {
            conn.Open();
            var cmd = new SqlCommand("UPDATE [Table] SET status = @status WHERE table_id = @tableId", conn);
            cmd.Parameters.AddWithValue("@status", newStatus);
            cmd.Parameters.AddWithValue("@tableId", tableId);
            cmd.ExecuteNonQuery();
        }
    }

    public IEnumerable<TableInfo> GetTablesWithStatus()
    {
        var tables = new List<TableInfo>();
        using (var conn = Chapeau25.ExtentionMethods.DatabaseHelper.GetConnection())
        {
            conn.Open();
            var cmd = new SqlCommand(@"
                SELECT 
                    t.table_id, 
                    t.table_number, 
                    t.status, 
                    t.capacity,
                    CASE WHEN EXISTS (
                        SELECT 1 FROM orders o
                        JOIN ORDER_ITEM oi ON o.order_id = oi.order_id
                        WHERE o.table_id = t.table_id AND oi.OrderItemStatus <> 'Served'
                    ) THEN 1 ELSE 0 END AS HasUnfinishedOrders
                FROM [Table] t
                WHERE t.status IS NOT NULL", conn);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    tables.Add(new TableInfo
                    {
                        TableId = reader.GetInt32(0),
                        TableNumber = reader.GetInt32(1),
                        Status = reader.GetString(2),
                        Capacity = reader.GetInt32(3),
                        HasUnfinishedOrders = reader.GetInt32(4) == 1
                    });
                }
            }
        }
        return tables;
    }

    public void ChangeTableStatus(int tableId, string newStatus)
    {
        using (var conn = Chapeau25.ExtentionMethods.DatabaseHelper.GetConnection())
        {
            conn.Open();
            var cmd = new SqlCommand("UPDATE [Table] SET status = @status WHERE table_id = @tableId", conn);
            cmd.Parameters.AddWithValue("@status", newStatus);
            cmd.Parameters.AddWithValue("@tableId", tableId);
            cmd.ExecuteNonQuery();
        }
    }

    public void SetOrderServed(int orderId)
    {
        using (var conn = Chapeau25.ExtentionMethods.DatabaseHelper.GetConnection())
        {
            conn.Open();
            var cmd = new SqlCommand("UPDATE ORDER_ITEM SET OrderItemStatus = 'Served' WHERE order_id = @orderId", conn);
            cmd.Parameters.AddWithValue("@orderId", orderId);
            cmd.ExecuteNonQuery();
        }
    }

    public void MarkOrderAsServed(int orderId)
    {
        using (var conn = Chapeau25.ExtentionMethods.DatabaseHelper.GetConnection())
        {
            conn.Open();
            var cmd = new SqlCommand("UPDATE ORDER_ITEM SET OrderItemStatus = 'Served' WHERE order_id = @orderId", conn);
            cmd.Parameters.AddWithValue("@orderId", orderId);
            cmd.ExecuteNonQuery();
        }
    }
}