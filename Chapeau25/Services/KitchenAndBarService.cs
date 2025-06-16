using Chapeau25.Enums;
using Chapeau25.Models;
using Chapeau25.Repositories;
using Chapeau25.Service;
using Chapeau25.ViewModel;
using NuGet.Protocol.Core.Types;

namespace Chapeau25.Services
{
    public class KitchenAndBarService : IKitchenAndBarService
    {
        private readonly IOrderRepository _KitchenAndBarRepo;

        public KitchenAndBarService(IOrderRepository Repo)
        {
            _KitchenAndBarRepo = Repo;
        }

        public List<BarAndKitchenViewModel> GetCurrentKitchenOrders()
        {
           return  _KitchenAndBarRepo.GetOrders(OrderFetchFilter.KitchenCurrent);
        }
        public List<BarAndKitchenViewModel> GetServedKitchenOrders()
        {
            return _KitchenAndBarRepo.GetOrders(OrderFetchFilter.KitchenServed);
        }
        public List<BarAndKitchenViewModel> GetCurrentBarOrders()
        { 
             return _KitchenAndBarRepo.GetOrders(OrderFetchFilter.BarCurrent); 
        }
        public List<BarAndKitchenViewModel> GetServedBarOrders()
        {

            return _KitchenAndBarRepo.GetOrders(OrderFetchFilter.BarServed);
        }
        public void ChangeOrderItemStatus(int orderItemId, OrderItemStatus orderItemStatus)
        {
            _KitchenAndBarRepo.ChangeOrderItemStatus(orderItemId, orderItemStatus);
        }
        public void ChangeKitchenCourseStatus(int orderId, string course, OrderItemStatus courseStatus)
        { 
            _KitchenAndBarRepo.ChangeKitchenCourseStatus(orderId, course, courseStatus);
        }
        public void ChangeEntireOrderStatus(int orderId,  bool isDrink,OrderItemStatus status)
        {
            _KitchenAndBarRepo.ChangeEntireOrderStatusByType(orderId, isDrink, status);
        }


    }
}
