namespace Chapeau25.Models
{
    public class TableInfo
    {
        public int TableId { get; set; }
        public int TableNumber { get; set; }
        public string Status { get; set; }
        public int Capacity { get; set; }
        public bool HasUnfinishedOrders { get; set; }
    }
}