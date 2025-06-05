using Chapeau25.Models;

namespace Chapeau25.Service
{
    public interface IKitchenAndBarService
    {
        List<Order> GetCurrentKitchenOrders();
        void ChangeKitchenOrderItemStatus(int orderItemId, OrderItemStatus orderItemStatus);

        void ChangeKitchenCourseStatus(int orderId, string Course, OrderItemStatus courseStatus);
        void ChangeWholeOrderStatus(int orderId, OrderItemStatus status);

        List<Order> GetServedKitchenOrders();

        public List<Order> GetCurrentBarOrders();

        void ChangeBarOrderItemStatus(int orderItemId, OrderItemStatus status);
        List<Order> GetServedBarOrders();
    }
}
