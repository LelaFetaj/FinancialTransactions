namespace FinancialTransactionsAPI.Models.Entities.Transactions
{
    public class TransactionFilter
    {
        public string? TransactionAmount { get; set; }
        public string? TransactionType { get; set; }
        public DateTimeOffset? TransactionDate { get; set; }
        public string? TransactionStatus { get; set; }
        public string? CustomerFullName { get; set; }
        public string? CustomerPhoneNumber { get; set; }
        public string? CustomerAddress { get; set; }
        public string? CustomerEmail { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}