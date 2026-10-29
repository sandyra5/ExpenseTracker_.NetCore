
using Microsoft.AspNetCore.Mvc;
using CoreExpenseTracker.Models;

namespace CoreExpenseTracker.Controllers
{
    public class TransactionController : Controller
    {
        RegisterDB dbobj = new RegisterDB();

        public IActionResult TransactionLoad(string type)
        {
            int? uid = HttpContext.Session.GetInt32("UserId");

            if (uid == null)
                return RedirectToAction("LoginLoad", "Login");

            ViewBag.Type = type;
            ViewBag.Categories = dbobj.GetCategoriesDB(type, uid.Value);

            return View();
        }

        [HttpPost]
        public IActionResult TransactionSave(TransactionModel obj)
        {
            int? uid = HttpContext.Session.GetInt32("UserId");

            if (uid == null)
                return RedirectToAction("LoginLoad", "Login");

            if (obj.TransactionType != "Income" &&
                obj.TransactionType != "Expense")
            {
                return BadRequest();
            }

            obj.TransDate = obj.TransDate.Date;

            string msg = dbobj.InsertTransactionDB(obj, uid.Value);

            if (msg == "Transaction added successfully")
            {
                return RedirectToAction("DashLoad", "Dashboard");
            }

            ViewBag.Message = msg;
            ViewBag.Type = obj.TransactionType;
            ViewBag.Categories = dbobj.GetCategoriesDB(obj.TransactionType, uid.Value);

            return View("TransactionLoad");
        }

    }
}
