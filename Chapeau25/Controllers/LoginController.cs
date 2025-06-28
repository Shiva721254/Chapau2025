using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using Chapeau25.Models;
using Chapeau25.ExtentionMethods; // Add this using directive
using Microsoft.Data.SqlClient; // Use Microsoft.Data.SqlClient for .NET 8

namespace Chapeau25.Controllers
{
    public class LoginController : Controller
    {
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

            // Hash the entered password using SHA256
            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(model.Password));
            var hashString = BitConverter.ToString(hash).Replace("-", "").ToLower();

            // Use DatabaseHelper to get the connection
            Employee employee = null;
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT employee_id, Username, Password, Role FROM Employee WHERE Username = @username", conn);
                cmd.Parameters.AddWithValue("@username", model.Username);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var dbHash = reader["Password"] as string;
                        if (dbHash == hashString)
                        {
                            employee = new Employee
                            {
                                Id = (int)reader["employee_id"],
                                Username = reader["Username"] as string,
                                HashedPassword = dbHash,
                                Role = reader["Role"] as string
                            };
                        }
                    }
                }
            }

            if (employee != null)
            {
                // Set session after successful login
                HttpContext.Session.SetInt32("EmployeeId", employee.Id);
                HttpContext.Session.SetString("Username", employee.Username ?? "");
                HttpContext.Session.SetString("Role", employee.Role ?? "");

                // Login successful, redirect or set session/cookie as needed
                return RedirectToAction("Orders", "Table");
            }

            ModelState.AddModelError("", "Invalid username or password.");
            return View(model);
        }

        [HttpPost]
        public IActionResult LogOff()
        {
            // Clear session or authentication as needed
            HttpContext.Session.Clear();
            // Redirect to login page
            return RedirectToAction("Login", "Login");
        }
    }
}