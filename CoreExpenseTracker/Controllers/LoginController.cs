
using Microsoft.AspNetCore.Mvc;
using CoreExpenseTracker.Models;

namespace CoreExpenseTracker.Controllers
{
    public class LoginController : Controller
    {
        RegisterDB dbobj = new RegisterDB();

        public IActionResult LoginLoad()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginClick(LoginModel objcls)
        {
            int count = dbobj.LoginDB(objcls);

            if (count == 1)
            {
                int uid = dbobj.GetUserIdDB(objcls);

                HttpContext.Session.SetInt32("UserId", uid);

                return RedirectToAction("DashLoad", "Dashboard");
            }

            ViewBag.Message = "Invalid username or password";
            return View("LoginLoad", objcls);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("LoginLoad", "Login");
        }

    }
}
