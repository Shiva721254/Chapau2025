using Chapeau25.Enums;
using Chapeau25.Models;

namespace Chapeau25.Repositories
{
    public interface IKitchenAndBarRepositories
    {
      
        public List<Order> GetOrders(OrderFetchFilter filter);

        void ChangeOrderItemStatus(int orderItemId, OrderItemStatus orderItemStatus);

        void ChangeKitchenCourseStatus(int orderId, string Course, OrderItemStatus courseStatus);
        void ChangeWholeOrderStatus(int orderId, OrderItemStatus status);

        
    }
}
