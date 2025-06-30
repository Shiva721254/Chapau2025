using Chapeau25.Enums;
using Chapeau25.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Chapeau25.Controllers
{
    public class TableController(ITableService tableService) : Controller
    {
        private readonly ITableService _tableService = tableService;

        public IActionResult Status()
        {
            var tables = _tableService.GetAllTables();
            return View(tables);
        }

        public IActionResult Orders()
        {
            var tableOrders = _tableService.GetTableOrders();
            return View("~/Views/Table/Orders.cshtml", tableOrders);
        }

        [HttpPost]
        public IActionResult ChangeStatus(int tableId, string newStatus)
        {
            try
            {
                _tableService.ChangeTableStatus(tableId, newStatus);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("ManageStatus");
        }

        [HttpPost]
        public IActionResult SetOrderServed(int orderId)
        {
            _tableService.SetOrderServed(orderId);
            return RedirectToAction("Orders");
        }

        public IActionResult ManageStatus()
        {
            var tables = _tableService.GetManageStatusTables();
            return View("ManageStatus", tables);
        }
    }
}