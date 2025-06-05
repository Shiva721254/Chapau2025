using Microsoft.AspNetCore.Http.HttpResults;

namespace Chapeau25.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string EmployeeName { get; set; }
        public OrderItemStatus OrderStatus { get; set; }
        public int TableNumber { get; set; }
        public DateTime OrderdTime { get; set; }
        public TimeSpan RunningTime
        {
            get
            {
                return DateTime.Now - OrderdTime;
            }
        }
        public List<OrderItem> OrderItems { get; set; }




        public Order()
        {
            

        }
     

       
        public Order(int orderId, string employeeName, OrderItemStatus orderStatus, int tableNumber, DateTime orderdTime, List<OrderItem> orderItems)
        {
            OrderId = orderId;
            EmployeeName = employeeName;
            OrderStatus = orderStatus;
            TableNumber = tableNumber;
            OrderdTime = orderdTime;
            OrderItems = new List<OrderItem>();
        }
    }
}
