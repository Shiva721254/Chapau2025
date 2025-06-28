using Chapeau25.Enums;
using Chapeau25.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Data.SqlClient; // Use Microsoft.Data.SqlClient
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using static NuGet.Packaging.PackagingConstants;

namespace Chapeau25.Controllers
{
    public class TableController : Controller
    {
        private readonly IConfiguration _configuration;

        public TableController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Status()
        {
            var tables = new List<TableInfo>();

            using (var conn = ExtentionMethods.DatabaseHelper.GetConnection())
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

            return View(tables);
        }

        public IActionResult Orders()
        {
            var tableOrders = new List<TableOrderStatusViewModel>();

            using (var conn = ExtentionMethods.DatabaseHelper.GetConnection())
            {
                conn.Open();
                // Get tables with running orders
                var cmd = new SqlCommand(@"
                    SELECT t.table_id, t.table_number, o.order_id, oi.OrderItemStatus, mi.name
                    FROM [Table] t
                    JOIN orders o ON t.table_id = o.table_id
                    JOIN ORDER_ITEM oi ON o.order_id = oi.order_id
                    JOIN MENU_ITEM mi ON oi.menuitem_id = mi.menuitem_id
                ", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.FieldCount < 5)
                        throw new InvalidOperationException("Query did not return the expected number of columns.");

                    var tableDict = new Dictionary<int, TableOrderStatusViewModel>();
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
                                TableNumber = tableNumber
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
                    tableOrders.AddRange(tableDict.Values);
                }
            }

            return View("~/Views/Table/Orders.cshtml", tableOrders);
        }

        // GET: Show table status management
        public IActionResult ManageStatus()
        {
            var tables = new List<TableInfo>();
            using (var conn = ExtentionMethods.DatabaseHelper.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT table_id, table_number, status FROM [Table]", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tables.Add(new TableInfo
                        {
                            TableId = reader.GetInt32(0),
                            TableNumber = reader.GetInt32(1),
                            Status = reader.GetString(2)
                        });
                    }
                }
            }
            return View("ManageStatus", tables);
        }

        // POST: Change table status
        [HttpPost]
        public IActionResult ChangeStatus(int tableId, string newStatus)
        {
            // Check for unfinished orders if setting to Available
            if (newStatus == "Available")
            {
                using (var conn = ExtentionMethods.DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    var cmd = new SqlCommand(@"
                        SELECT COUNT(*) FROM orders o
                        JOIN ORDER_ITEM oi ON o.order_id = oi.order_id
                        WHERE o.table_id = @tableId AND oi.OrderItemStatus <> 'Served'
                    ", conn);
                    cmd.Parameters.AddWithValue("@tableId", tableId);
                    int unfinishedOrders = (int)cmd.ExecuteScalar();
                    if (unfinishedOrders > 0)
                    {
                        TempData["Error"] = "Cannot set to Available: Unfinished orders exist.";
                        return RedirectToAction("ManageStatus");
                    }
                }
            }

            // Update table status
            using (var conn = ExtentionMethods.DatabaseHelper.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("UPDATE [Table] SET status = @status WHERE table_id = @tableId", conn);
                cmd.Parameters.AddWithValue("@status", newStatus);
                cmd.Parameters.AddWithValue("@tableId", tableId);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("ManageStatus");
        }

        [HttpPost]
        public IActionResult SetOrderServed(int orderId)
        {
            using (var conn = ExtentionMethods.DatabaseHelper.GetConnection())
            {
                conn.Open();
                // Only update if current status is 'Ready'
                var cmd = new SqlCommand(@"
                    UPDATE ORDER_ITEM
                    SET OrderItemStatus = 'Served'
                    WHERE order_id = @orderId AND OrderItemStatus = 'Ready'
                ", conn);
                cmd.Parameters.AddWithValue("@orderId", orderId);
                cmd.ExecuteNonQuery();
            }
            // Refresh the Orders view
            return RedirectToAction("Orders");
        }
    }
}