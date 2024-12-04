using FinancialTransactionsAPI.Models.Entities.Transactions;

namespace FinancialTransactionsAPI.Models.DTOs.Transactions
{
    public class CreateTransactionDto
    {
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public string? Description { get; set; }
        public Status Status { get; set; }
        public Guid CustomerId { get; set; }
    }
}