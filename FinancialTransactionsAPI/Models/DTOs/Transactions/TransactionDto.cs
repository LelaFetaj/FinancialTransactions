using FinancialTransactionsAPI.Models.Entities.Transactions;

namespace FinancialTransactionsAPI.Models.DTOs.Transactions
{
    public class TransactionDto
    {
        public Guid TransactionId { get; set; }
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTimeOffset TransactionCreateDate { get; set; }
        public DateTimeOffset TransactionUpdateDate { get; set; }
        public string? Description { get; set; }
        public Status Status { get; set; }
        public bool IsActive { get; set; } = true;
        public Guid CustomerId { get; set; }
    }
}