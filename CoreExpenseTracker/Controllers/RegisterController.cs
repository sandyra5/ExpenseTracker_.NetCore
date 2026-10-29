using Microsoft.AspNetCore.Mvc;
using CoreExpenseTracker.Models;
namespace CoreExpenseTracker.Controllers
{
    public class RegisterController : Controller
    {
        RegisterDB dbobj=new RegisterDB();
        public IActionResult RegisterLoad()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RegisterClick(RegisterModel objcls)
        {
            string msg = dbobj.InsertDB(objcls);
            ViewBag.Message = msg;

            return View("RegisterLoad", objcls);
        }
    }
}
