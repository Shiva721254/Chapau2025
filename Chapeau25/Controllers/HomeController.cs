using System.Diagnostics;
using Chapeau25.Models;
using Chapeau25.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Chapeau25.Controllers
{
    public class HomeController : Controller
    {
       
        public IActionResult Index()
        {
            return View();
        }

      
        public IActionResult Privacy()
        {
            return View();
        }

       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

       

        
    }
}
