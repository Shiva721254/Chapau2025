using Chapeau25.Models;

public class TableService(ITableRepository tableRepository) : ITableService
{
    private readonly ITableRepository _tableRepository = tableRepository;

    public IEnumerable<TableInfo> GetAllTables()
    {
        return _tableRepository.GetAllTables();
    }

    public IEnumerable<TableOrderStatusViewModel> GetTableOrders()
    {
        return _tableRepository.GetTableOrders();
    }

    public void ChangeTableStatus(int tableId, string newStatus)
    {
        // Business logic: Only allow change if no unfinished orders  
        if (newStatus == "Available" && _tableRepository.HasUnfinishedOrders(tableId))
            throw new InvalidOperationException("Cannot set to Available: Unfinished orders exist.");
        _tableRepository.UpdateTableStatus(tableId, newStatus);
    }

    public void SetOrderServed(int orderId)
    {
        // Implementation for SetOrderServed  
        _tableRepository.MarkOrderAsServed(orderId);
    }

    public object GetManageStatusTables()
    {
        // Use repository to get tables with their status
        var tablesWithStatus = _tableRepository.GetTablesWithStatus();
        return tablesWithStatus;
    }
}