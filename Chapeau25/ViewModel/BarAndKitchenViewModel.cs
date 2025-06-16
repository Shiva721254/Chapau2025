using Chapeau25.Enums;
using Chapeau25.Models;

namespace Chapeau25.ViewModel
{
    public class BarAndKitchenViewModel
    {

        public int OrderId { get; set; }
        public string EmployeeName { get; set; }
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
        public BarAndKitchenViewModel()
        {


        }

        public BarAndKitchenViewModel(int orderId, string employeeName,  int tableNumber, DateTime orderdTime, List<OrderItem> orderItems)
        {
            OrderId = orderId;
            EmployeeName = employeeName;
            TableNumber = tableNumber;
            OrderdTime = orderdTime;
            OrderItems = new List<OrderItem>();
        }
    }
}