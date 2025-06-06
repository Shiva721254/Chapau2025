using Chapeau25.Enums;
using Chapeau25.Models;
using Chapeau25.Repositories;
using Chapeau25.Service;
using Microsoft.AspNetCore.Mvc;

namespace Chapeau25.Controllers
{
    public class KitchenAndBarController : Controller
    {
        private readonly IKitchenAndBarService _kitchenBarService;

        public KitchenAndBarController(IKitchenAndBarService kitchenBarService)
        {
            _kitchenBarService = kitchenBarService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CurrentKitchenOrders()
        {
            List<Order> orders = _kitchenBarService.GetCurrentKitchenOrders();
            return View(orders);
        }
        public IActionResult ServedKitchenOrders()
        {
            List<Order> orders = _kitchenBarService.GetServedKitchenOrders();
            return View(orders);
        }
        public IActionResult CurrentBarOrders()
        {
            List<Order> orders = _kitchenBarService.GetCurrentBarOrders();
            return View(orders);
        }
        public IActionResult ServedBarOrders()
        {
            List<Order> orders = _kitchenBarService.GetServedBarOrders();
            return View(orders);
        }

        [HttpPost]
        public IActionResult ChangeKitchenOrderItemStatus(int orderItemId, OrderItemStatus orderItemStatus)
        {
            _kitchenBarService.ChangeOrderItemStatus(orderItemId, orderItemStatus);
            return RedirectToAction("CurrentKitchenOrders");
        }
        [HttpPost]
        public IActionResult ChangeCourseStatus(int orderId, string course, OrderItemStatus courseStatus)
        {

            _kitchenBarService.ChangeKitchenCourseStatus(orderId, course, courseStatus);
                return RedirectToAction("CurrentKitchenOrders"); 
           
        }

        [HttpPost]
        public IActionResult ChangeFoodOrDrinkStatus(int orderId, bool isDrink, OrderItemStatus orderItemStatus)
        {
            _kitchenBarService.ChangeEntireOrderStatus(orderId,isDrink, orderItemStatus);
            return RedirectToAction("CurrentKitchenOrders");
        }

        [HttpPost]
        public IActionResult GetBackServedOrder(int orderId, string course, OrderItemStatus courseStatus)
        {

            _kitchenBarService.ChangeKitchenCourseStatus(orderId, course, courseStatus);
            return RedirectToAction("ServedKitchenOrders"); 

        }

        [HttpPost]
        public IActionResult ChangeBarOrderItemStatus(int orderItemId, OrderItemStatus orderItemStatus)
        {
            _kitchenBarService.ChangeOrderItemStatus(orderItemId, orderItemStatus);
            return RedirectToAction("CurrentBarOrders");
        }

       
    }
}
