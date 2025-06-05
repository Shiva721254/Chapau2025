namespace Chapeau25.Models
{
    public class OrderItem
    {
        public int OrderItemID { get; set; }
        public string ItemName { get; set; }
        public decimal ItemPrice { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; }
       // public string Comment { get; set; }
        public decimal Price { 
            get
            {
                return ItemPrice * Quantity;
            }
                }
       public OrderItemStatus OrderItemStatus { get; set; }
        public string CourseStatus
        {
            get
            {
                if (OrderItemStatus == OrderItemStatus.Ordered)
                {
                    return "Ordered";
                }
                else if (OrderItemStatus == OrderItemStatus.Preparing)
                {
                    return "Preparing";
                }
                else if (OrderItemStatus == OrderItemStatus.Ready)
                {
                    return "Ready";
                }
                else
                {
                    return "Served";
                }
            }
        }

        public OrderItem()
        {

        }
     

        public OrderItem(int orderItemID, string itemName, decimal itemPrice, int quantity, OrderItemStatus orderItemStatus, string type)    
        {
            OrderItemID = orderItemID;
            ItemName = itemName;
            ItemPrice = itemPrice;
            Quantity = quantity;
            OrderItemStatus = orderItemStatus;
            Type = type;
          //  Comment = comment;

        }
    }
}
