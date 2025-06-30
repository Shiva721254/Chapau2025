using Chapeau25.Models;
using System.Collections.Generic;

public interface ITableRepository
{
    public IEnumerable<TableInfo> GetAllTables();

    public IEnumerable<TableOrderStatusViewModel> GetTableOrders();

    public void ChangeTableStatus(int tableId, string newStatus);

    public void SetOrderServed(int orderId);

    bool HasUnfinishedOrders(int tableId);

    void UpdateTableStatus(int tableId, string newStatus);

    void MarkOrderAsServed(int orderId);

    IEnumerable<TableInfo> GetTablesWithStatus();
}