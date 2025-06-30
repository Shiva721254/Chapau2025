using Microsoft.AspNetCore.Mvc;
using Chapeau25.Models;
using Chapeau25.ExtentionMethods; // Add this using directive

namespace Chapeau25.Controllers
{
    public class LoginController(ILoginService loginService) : Controller
    {
        private readonly ILoginService _loginService = loginService;

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Login model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var employee = _loginService.Authenticate(model.Username, model.Password);

            if (employee != null)
            {
                HttpContext.Session.SetInt32("EmployeeId", employee.Id);
                HttpContext.Session.SetString("Username", employee.Username ?? "");
                HttpContext.Session.SetString("Role", employee.Role ?? "");
                return RedirectToAction("Orders", "Table");
            }

            ModelState.AddModelError("", "Invalid username or password.");
            return View(model);
        }

        [HttpPost]
        public IActionResult LogOff()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}