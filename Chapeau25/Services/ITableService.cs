using Chapeau25.Models;
using System.Collections.Generic;

public interface ITableService
{
    IEnumerable<TableInfo> GetAllTables();
    IEnumerable<TableOrderStatusViewModel> GetTableOrders();
    void ChangeTableStatus(int tableId, string newStatus);
    void SetOrderServed(int orderId);
    object GetManageStatusTables();
}