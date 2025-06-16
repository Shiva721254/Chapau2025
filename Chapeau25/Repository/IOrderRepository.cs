using Chapeau25.Enums;
using Chapeau25.Models;
using Chapeau25.ViewModel;

namespace Chapeau25.Repositories
{
    public interface IOrderRepository
    {
      
        public List<BarAndKitchenViewModel> GetOrders(OrderFilter filter);

        void ChangeOrderItemStatus(int orderItemId, OrderItemStatus orderItemStatus);

        void ChangeKitchenCourseStatus(int orderId, string Course, OrderItemStatus courseStatus);
        void ChangeEntireOrderStatusByType(int orderId, bool isDrink, OrderItemStatus status);



    }
}
