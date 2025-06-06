using Chapeau25.Enums;
using Chapeau25.Models;
using Chapeau25.Repositories;
using Chapeau25.Service;

namespace Chapeau25.Services
{
    public class KitchenAndBarService : IKitchenAndBarService
    {
        private readonly IKitchenAndBarRepositories _KitchenAndBarRepo;

        public KitchenAndBarService(IKitchenAndBarRepositories Repo)
        {
            _KitchenAndBarRepo = Repo;
        }

        public List<Order> GetCurrentKitchenOrders()
        {
           return  _KitchenAndBarRepo.GetOrders(OrderFetchFilter.KitchenCurrent);
        }
        public List<Order> GetServedKitchenOrders()
        {
            return _KitchenAndBarRepo.GetOrders(OrderFetchFilter.KitchenServed);
        }
        public List<Order> GetCurrentBarOrders()
        { 
             return _KitchenAndBarRepo.GetOrders(OrderFetchFilter.BarCurrent); 
        }
        public List<Order> GetServedBarOrders()
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
        public void ChangeWholeOrderStatus(int orderId, OrderItemStatus status)
        { 
             _KitchenAndBarRepo.ChangeWholeOrderStatus(orderId, status);
        }
       
       
    }
}
