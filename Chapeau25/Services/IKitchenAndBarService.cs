using Chapeau25.Enums;
using Chapeau25.Models;

namespace Chapeau25.Service
{
    public interface IKitchenAndBarService
    {
        List<Order> GetCurrentKitchenOrders();
        List<Order> GetServedKitchenOrders();
        public List<Order> GetCurrentBarOrders();
        List<Order> GetServedBarOrders();
        void ChangeOrderItemStatus(int orderItemId, OrderItemStatus orderItemStatus);
        void ChangeKitchenCourseStatus(int orderId, string Course, OrderItemStatus courseStatus);
        void ChangeEntireOrderStatus(int orderId,bool isDrink, OrderItemStatus status);
       
    }
}
