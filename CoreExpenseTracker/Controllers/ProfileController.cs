using Microsoft.AspNetCore.Mvc;
using CoreExpenseTracker.Models;

namespace CoreExpenseTracker.Controllers
{
    public class ProfileController : Controller
    {
        RegisterDB dbobj = new RegisterDB();

        public IActionResult ProfileLoad()
        {
            int? uid = HttpContext.Session.GetInt32("UserId");

            if (uid == null)
                return RedirectToAction("LoginLoad", "Login");

            UserProfile obj = dbobj.GetUserProfileDB(uid.Value);

            return View(obj);
        }
        [HttpPost]
        public IActionResult ProfileUpdate(UserProfile obj)
        {
            int? uid = HttpContext.Session.GetInt32("UserId");

            if (uid == null)
                return RedirectToAction("LoginLoad", "Login");

            UserProfile oldProfile = dbobj.GetUserProfileDB(uid.Value);

            // Keep the existing profile details unchanged
            obj.UserId = uid.Value;
            obj.Photo = oldProfile.Photo;
            obj.Name = oldProfile.Name;
            obj.Email = oldProfile.Email;
            obj.Username = oldProfile.Username;

            // Update only the profession
            string result = dbobj.UpdateProfileDB(obj);

            if (result == "Profile updated successfully")
                return RedirectToAction("ProfileLoad");

            ViewBag.Message = result;
            return View("ProfileLoad", obj);
        }
    }
}
