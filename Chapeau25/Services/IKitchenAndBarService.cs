using Chapeau25.Enums;
using Chapeau25.Models;
using Chapeau25.ViewModel;

namespace Chapeau25.Service
{
    public interface IKitchenAndBarService
    {
        List<BarAndKitchenViewModel> GetCurrentKitchenOrders();
        List<BarAndKitchenViewModel> GetServedKitchenOrders();
        public List<BarAndKitchenViewModel> GetCurrentBarOrders();
        List<BarAndKitchenViewModel> GetServedBarOrders();
        void ChangeOrderItemStatus(int orderItemId, OrderItemStatus orderItemStatus);
        void ChangeKitchenCourseStatus(int orderId, string Course, OrderItemStatus courseStatus);
        void ChangeEntireOrderStatus(int orderId,bool isDrink, OrderItemStatus status);
       
    }
}
