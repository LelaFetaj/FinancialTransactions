using FinancialTransactionsAPI.Models.Entities.Customers;

namespace FinancialTransactionsAPI.Models.Entities.Transactions
{
    public class Transaction
    {
        public Guid TransactionId { get; set; }
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTimeOffset TransactionCreateDate { get; set; }
        public string? Description { get; set; }
        public Status Status { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTimeOffset TransactionUpdateDate { get; set; }
        public Guid CustomerId { get; set; }
        public required Customer Customer { get; set; }
    }
}