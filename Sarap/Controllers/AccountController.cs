using Microsoft.AspNetCore.Mvc;

namespace Sarap.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (email == "admin@sarap.com" && password == "1234")
            {
                TempData["Mensaje"] = "Sesión iniciada correctamente.";
                return RedirectToAction("Index", "Home");
            }

            TempData["Error"] = "Credenciales inválidas. Inténtalo nuevamente.";
            return View();
        }

        public IActionResult Logout()
        {
            TempData["Mensaje"] = "Sesión cerrada.";
            return RedirectToAction("Login", "Account");
        }
    }
}
