using Chapeau25.Models;
using Chapeau25.Repositories;
using Chapeau25.Service;

namespace Chapeau25.Services
{
    public class KitchenAndBarService : IKitchenAndBarService
    {
        private readonly IKitchenAndBarRepositories _KitchenAndBarRepo;

        public KitchenAndBarService(IKitchenAndBarRepositories ServiceRepo)
        {
            _KitchenAndBarRepo = ServiceRepo;
        }

        public List<Order> GetCurrentKitchenOrders() => _KitchenAndBarRepo.GetCurrentKitchenOrders();

        public void ChangeKitchenOrderItemStatus(int orderItemId, OrderItemStatus orderItemStatus)
        {
            _KitchenAndBarRepo.ChangeKitchenOrderItemStatus(orderItemId, orderItemStatus);
        }
        public void ChangeKitchenCourseStatus(int orderId, string course, OrderItemStatus courseStatus)
        { 
            _KitchenAndBarRepo.ChangeKitchenCourseStatus(orderId, course, courseStatus);
        }
        public void ChangeWholeOrderStatus(int orderId, OrderItemStatus status)
        { 
             _KitchenAndBarRepo.ChangeWholeOrderStatus(orderId, status);
        }
        public List<Order> GetServedKitchenOrders() => _KitchenAndBarRepo.GetServedKitchenOrders();

        public List<Order> GetCurrentBarOrders() => _KitchenAndBarRepo.GetCurrentBarOrders();

        public void ChangeBarOrderItemStatus(int orderItemId, OrderItemStatus orderItemStatus)
        { 
             _KitchenAndBarRepo.ChangeBarOrderItemStatus(orderItemId, orderItemStatus);
        }
        public List<Order> GetServedBarOrders() => _KitchenAndBarRepo.GetServedBarOrders();
    }
}
