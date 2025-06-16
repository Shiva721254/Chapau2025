using Chapeau25.Enums;
using Chapeau25.Models;
using Chapeau25.Repositories;
using Chapeau25.Service;
using Chapeau25.ViewModel;
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
          return  View();
        }

        [HttpGet]
        public IActionResult CurrentKitchenOrders()
        {
            try
            {
                var orders = _kitchenBarService.GetCurrentKitchenOrders();
                return View(orders);
            }
            catch
            {
                TempData["ErrorMessage"] = "Unable to load current kitchen orders. Please try again.";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult ServedKitchenOrders()
        {
            try
            {
                var orders = _kitchenBarService.GetServedKitchenOrders();
                return View(orders);
            }
            catch
            {
                TempData["ErrorMessage"] = "Unable to load served kitchen orders.";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult CurrentBarOrders()
        {
            try
            {
                var orders = _kitchenBarService.GetCurrentBarOrders();
                return View(orders);
            }
            catch
            {
                TempData["ErrorMessage"] = "Unable to load current bar orders.";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult ServedBarOrders()
        {
            try
            {
                var orders = _kitchenBarService.GetServedBarOrders();
                return View(orders);
            }
            catch
            {
                TempData["ErrorMessage"] = "Unable to load served bar orders.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult ChangeKitchenOrderItemStatus(int orderItemId, OrderItemStatus orderItemStatus)
        {
            try
            {
                _kitchenBarService.ChangeOrderItemStatus(orderItemId, orderItemStatus);
                TempData["SuccessMessage"] = $"Order item #{orderItemId} updated.";
            }
            catch
            {
                TempData["ErrorMessage"] = $"Error updating order item #{orderItemId}.";
            }
            return RedirectToAction("CurrentKitchenOrders");
        }

        [HttpPost]
        public IActionResult ChangeCourseStatus(int orderId, string course, OrderItemStatus courseStatus)
        {
            try
            {
                _kitchenBarService.ChangeKitchenCourseStatus(orderId, course, courseStatus);
                TempData["SuccessMessage"] = $"Course '{course}' for order #{orderId} updated.";
            }
            catch
            {
                TempData["ErrorMessage"] = $"Error updating course '{course}' for order #{orderId}.";
            }
            return RedirectToAction("CurrentKitchenOrders");
        }

        [HttpPost]
        public IActionResult ChangeEntireFoodStatus(int orderId, bool isDrink, OrderItemStatus orderItemStatus)
        {
            try
            {
                _kitchenBarService.ChangeEntireOrderStatus(orderId, isDrink, orderItemStatus);
                TempData["SuccessMessage"] = $"Food order #{orderId} updated to {orderItemStatus}.";
            }
            catch
            {
                TempData["ErrorMessage"] = $"Error updating food order #{orderId}.";
            }
            return RedirectToAction("CurrentKitchenOrders");
        }

        [HttpPost]
        public IActionResult ChangeEntireDrinkStatus(int orderId, bool isDrink, OrderItemStatus orderItemStatus)
        {
            try
            {
                _kitchenBarService.ChangeEntireOrderStatus(orderId, isDrink, orderItemStatus);
                TempData["SuccessMessage"] = $"Drink order #{orderId} updated to {orderItemStatus}.";
            }
            catch
            {
                TempData["ErrorMessage"] = $"Error updating drink order #{orderId}.";
            }
            return RedirectToAction("CurrentBarOrders");
        }

        [HttpPost]
        public IActionResult GetBackServedOrder(int orderId, string course, OrderItemStatus courseStatus)
        {
            try
            {
                _kitchenBarService.ChangeKitchenCourseStatus(orderId, course, courseStatus);
                TempData["SuccessMessage"] = $"Course '{course}' for order #{orderId} set back.";
            }
            catch
            {
                TempData["ErrorMessage"] = "Error reverting course  for order .";
            }
            return RedirectToAction("ServedKitchenOrders");
        }

        [HttpPost]
        public IActionResult ChangeBarOrderItemStatus(int orderItemId, OrderItemStatus orderItemStatus)
        {
            try
            {
                _kitchenBarService.ChangeOrderItemStatus(orderItemId, orderItemStatus);
                TempData["SuccessMessage"] = $"Bar item #{orderItemId} updated.";
            }
            catch
            {
                TempData["ErrorMessage"] = $"Error updating bar item #{orderItemId}.";
            }
            return RedirectToAction("CurrentBarOrders");
        }

        [HttpPost]
        public IActionResult GetBackBarOrderItemStatus(int orderItemId, OrderItemStatus orderItemStatus)
        {
            try
            {
                _kitchenBarService.ChangeOrderItemStatus(orderItemId, orderItemStatus);
                TempData["SuccessMessage"] = $"Bar item #{orderItemId} restored.";
            }
            catch
            {
                TempData["ErrorMessage"] = $"Error restoring bar item #{orderItemId}.";
            }
            return RedirectToAction("ServedBarOrders");
        }
    }

}
