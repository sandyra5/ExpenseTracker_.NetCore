namespace CoreExpenseTracker.Models
{
    public class TransactionModel
    {
        public int TransactionId { get; set; }

        public int CategoryId { get; set; }

        public decimal Amount { get; set; }

        public string TransactionType { get; set; } = "";

        public DateTime TransDate { get; set; }

        public string Description { get; set; } = "";
    }
}
