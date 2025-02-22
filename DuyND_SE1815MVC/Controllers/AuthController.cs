using DuyND_SE1815_Data.Entities;
using DuyND_SE1815_Services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DuyND_SE1815MVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        // GET: Auth/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Auth/Login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _authService.AuthenticateUser(email, password);

            if (user == null)
            {
                ViewBag.Error = "Invalid email or password";
                return View();
            }

            // Lưu user vào session
            HttpContext.Session.SetInt32("UserId", user.AccountId);
            HttpContext.Session.SetInt32("UserRole", user.AccountRole ?? -1);
            if (user.AccountRole == 0)
            {
                return RedirectToAction("Index", "Account");
            }
            if (user.AccountRole == 1)
            {
                return RedirectToAction("Index", "Category");
            }
            if (user.AccountRole == 2)
            {
                return RedirectToAction("Manage", "NewsArticle");
            }
            return RedirectToAction("Index", "NewsArticle");
        }

        // Đăng xuất
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
