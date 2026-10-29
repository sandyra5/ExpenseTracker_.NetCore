using CoreExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreExpenseTracker.Controllers
{
    public class DashboardController : Controller
    {
        RegisterDB dbobj = new RegisterDB();
        public IActionResult DashLoad()
        {
            int? uid = HttpContext.Session.GetInt32("UserId");

            if (uid == null)
            {
                return RedirectToAction("LoginLoad", "Login");
            }

            DashboardModel obj = dbobj.GetDashboardDB(uid.Value);
            ViewBag.ExpenseCategories =dbobj.GetMonthlyExpenseCategoryDB(uid.Value);
            ViewBag.RecentTransactions =dbobj.GetRecentTransactionsDB(uid.Value);

            return View(obj);
        }
    }
}
