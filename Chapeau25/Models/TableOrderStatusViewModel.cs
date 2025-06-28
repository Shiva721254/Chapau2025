using Chapeau25.Enums;
using System.Collections.Generic;

namespace Chapeau25.Models
{
    public class TableOrderStatusViewModel
    {
        public int TableId { get; set; }
        public int TableNumber { get; set; }
        public List<OrderStatusInfo> Orders { get; set; } = new();
    }

    public class OrderStatusInfo
    {
        public int OrderId { get; set; }
        public OrderItemStatus Status { get; set; }
        public string StatusDisplay => Status.ToString().Replace("ReadyToBeServed", "Ready to be served").Replace("BeingPrepared", "Being prepared");
        public required string Name { get; set; }
    }
}