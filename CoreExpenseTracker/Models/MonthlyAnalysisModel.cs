using System.Collections.Generic;
namespace CoreExpenseTracker.Models
{
    public class MonthlyAnalysisModel
    {
        public string Month { get; set; } = "";

        public decimal MonthlyIncome { get; set; }

        public decimal MonthlyExpenses { get; set; }

        public decimal Balance
        {
            get { return MonthlyIncome - MonthlyExpenses; }
        }

        public string AISummary { get; set; } = "";
        public Dictionary<string, decimal> Categories { get; set; } = new Dictionary<string, decimal>();
    }
}
