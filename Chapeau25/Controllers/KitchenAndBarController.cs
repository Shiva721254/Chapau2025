using Chapeau25.Models;
using Chapeau25.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Chapeau25.Controllers
{
    public class KitchenAndBarController : Controller
    {
        private readonly IKitchenAndBarRepositories _kitchenBarController;

        public KitchenAndBarController(IKitchenAndBarRepositories kitchenBarOrder)
        {
            _kitchenBarController = kitchenBarOrder;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CurrentKitchenOrders()
        {
            List<Order> orders = _kitchenBarController.GetCurrentKitchenOrders();
            return View(orders);
        }

        [HttpPost]
        public IActionResult ChangeKitchenOrderItemStatus(int orderItemId, OrderItemStatus orderItemStatus)
        {
            _kitchenBarController.ChangeKitchenOrderItemStatus(orderItemId, orderItemStatus);
            return RedirectToAction("CurrentKitchenOrders");
        }
        [HttpPost]
        public IActionResult ChangeCourseStatus(int orderId, string course, OrderItemStatus courseStatus)
        {
           
            _kitchenBarController.ChangeKitchenCourseStatus(orderId, course, courseStatus);
                return RedirectToAction("CurrentKitchenOrders"); 
           
        }

     
        [HttpPost]
        public IActionResult ChangeWholeOrderStatus(int orderId, OrderItemStatus orderStatus)
        {
            _kitchenBarController.ChangeWholeOrderStatus(orderId, orderStatus);
            return RedirectToAction("CurrentKitchenOrders"); 
        }

        public IActionResult ServedKitchenOrders()
        {
            List<Order> orders = _kitchenBarController.GetServedKitchenOrders();
            return View(orders);
        }

        [HttpPost]
        public IActionResult GetBackServedOrder(int orderId, string course, OrderItemStatus courseStatus)
        {

            _kitchenBarController.ChangeKitchenCourseStatus(orderId, course, courseStatus);
            return RedirectToAction("ServedKitchenOrders"); 

        }

        public IActionResult CurrentBarOrders()
        {
            List<Order> orders = _kitchenBarController.GetCurrentBarOrders();
            return View(orders);
        }

        [HttpPost]
        public IActionResult ChangeBarOrderItemStatus(int orderItemId, OrderItemStatus orderItemStatus)
        {
            _kitchenBarController.ChangeKitchenOrderItemStatus(orderItemId, orderItemStatus);
            return RedirectToAction("CurrentBarOrders");
        }

        public IActionResult ServedBarOrders()
        {
            List<Order> orders = _kitchenBarController.GetServedBarOrders();
            return View(orders);
        }
    }
}
