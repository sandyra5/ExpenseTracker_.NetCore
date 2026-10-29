namespace CoreExpenseTracker.Models
{
    public class DashboardModel
    {
        public decimal MonthlyIncome { get; set; }
        public decimal MonthlyExpenses { get; set; }
        public decimal TodayExpenses { get; set; }

        public decimal MonthlyBalance
        {
            get { return MonthlyIncome - MonthlyExpenses; }
        }
    }
}
