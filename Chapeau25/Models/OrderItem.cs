﻿using Chapeau25.Enums;

namespace Chapeau25.Models
{
    public class OrderItem
    {
        public int OrderItemID { get; set; }
        public string ItemName { get; set; }
        public decimal ItemPrice { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; }
        public string Comment { get; set; }
        public decimal Price { 
            get
            {
                return ItemPrice * Quantity;
            }
        }
       public OrderItemStatus OrderItemStatus { get; set; }
       

        public OrderItem()
        {

        }
     

        public OrderItem(int orderItemID, string itemName, decimal itemPrice, int quantity, OrderItemStatus orderItemStatus, string type, string comment)    
        {
            OrderItemID = orderItemID;
            ItemName = itemName;
            ItemPrice = itemPrice;
            Quantity = quantity;
            OrderItemStatus = orderItemStatus;
            Type = type;
            Comment = comment;

        }
    }
}
