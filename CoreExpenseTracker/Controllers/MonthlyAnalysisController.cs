
using Microsoft.AspNetCore.Mvc;
using CoreExpenseTracker.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System;
using System.Threading.Tasks;

namespace CoreExpenseTracker.Controllers
{
    public class MonthlyAnalysisController : Controller
    {
        RegisterDB dbobj = new RegisterDB();

        // Display the monthly analysis page
        public IActionResult AnalysisLoad(string? month)
        {
            int? uid = HttpContext.Session.GetInt32("UserId");

            if (uid == null)
                return RedirectToAction("LoginLoad", "Login");

            DateTime selectedDate;

            if (!DateTime.TryParseExact(
                month,
                "yyyy-MM",
                null,
                System.Globalization.DateTimeStyles.None,
                out selectedDate))
            {
                selectedDate = DateTime.Now;
            }

            MonthlyAnalysisModel obj = dbobj.GetMonthlyAnalysisDB(
                uid.Value,
                selectedDate.Month,
                selectedDate.Year
            );

            return View(obj);
        }

        // Generate the AI summary
        [HttpPost]
        public async Task<IActionResult> GenerateSummary(string month)
        {
            int? uid = HttpContext.Session.GetInt32("UserId");

            if (uid == null)
                return RedirectToAction("LoginLoad", "Login");

            if (!DateTime.TryParseExact(
                month,
                "yyyy-MM",
                null,
                System.Globalization.DateTimeStyles.None,
                out DateTime selectedDate))
            {
                return BadRequest("Invalid month selected.");
            }

            MonthlyAnalysisModel obj = dbobj.GetMonthlyAnalysisDB(
                uid.Value,
                selectedDate.Month,
                selectedDate.Year
            );

            var requestData = new
            {
                month = selectedDate.ToString("MMMM yyyy"),
                income = obj.MonthlyIncome,
                expenses = obj.MonthlyExpenses,
                categories = obj.Categories
            };

            try
            {
                using HttpClient client = new HttpClient();

                HttpResponseMessage response =
                    await client.PostAsJsonAsync(
                        "http://127.0.0.1:8000/monthly-summary",
                        requestData
                    );

                response.EnsureSuccessStatusCode();

                var result = await response.Content
                    .ReadFromJsonAsync<AIResponse>();

                obj.AISummary = result?.summary
                    ?? "The AI returned an empty summary.";
            }
            catch (Exception)
            {
                obj.AISummary =
                    "Unable to generate the AI summary. " +
                    "Please make sure the Python AI service is running.";
            }

            return View("AnalysisLoad", obj);
        }

        public class AIResponse
        {
            public string month { get; set; } = "";
            public string summary { get; set; } = "";
        }
    }
}
