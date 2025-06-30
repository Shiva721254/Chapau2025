using Chapeau25.Enums;
using Chapeau25.Models;
using Chapeau25.Repositories;
using Chapeau25.Service;
using Chapeau25.ViewModel;
using NuGet.Protocol.Core.Types;

namespace Chapeau25.Services
{
    public class KitchenAndBarService(IOrderRepository Repo) : IKitchenAndBarService
    {
        private readonly IOrderRepository _OrderRepo = Repo;

        public List<BarAndKitchenViewModel> GetCurrentKitchenOrders()
        {
           return _OrderRepo.GetOrders(OrderFilter.KitchenCurrent);
        }
        public List<BarAndKitchenViewModel> GetServedKitchenOrders()
        {
            return _OrderRepo.GetOrders(OrderFilter.KitchenServed);
        }
        public List<BarAndKitchenViewModel> GetCurrentBarOrders()
        { 
             return _OrderRepo.GetOrders(OrderFilter.BarCurrent); 
        }
        public List<BarAndKitchenViewModel> GetServedBarOrders()
        {

            return _OrderRepo.GetOrders(OrderFilter.BarServed);
        }
        public void ChangeOrderItemStatus(int orderItemId, OrderItemStatus orderItemStatus)
        {
            _OrderRepo.ChangeOrderItemStatus(orderItemId, orderItemStatus);
        }
        public void ChangeKitchenCourseStatus(int orderId, string course, OrderItemStatus courseStatus)
        {
            _OrderRepo.ChangeKitchenCourseStatus(orderId, course, courseStatus);
        }
        public void ChangeEntireOrderStatus(int orderId,  bool isDrink,OrderItemStatus status)
        {
            _OrderRepo.ChangeEntireOrderStatusByType(orderId, isDrink, status);
        }


    }
}
