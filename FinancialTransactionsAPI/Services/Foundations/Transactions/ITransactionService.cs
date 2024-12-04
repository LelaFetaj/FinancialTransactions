using FinancialTransactionsAPI.Models.DTOs.Transactions;
using FinancialTransactionsAPI.Models.Entities.Transactions;

namespace FinancialTransactionsAPI.Services.Foundations.Transactions
{
    public interface ITransactionService
    {
        Task<TransactionDto> CreateTransactionAsync(CreateTransactionDto createTransactionDTO);
        Task<TransactionDto> GetTransactionByIdAsync(Guid id);
        Task<List<Transaction>> GetAllTransactionsAsync();
        Task<TransactionDto> ModifyTransactionAsync(UpdateTransactionDto updateTransactionDto);
        Task RemoveTransactionAsync(Guid transactionId);
        Task SoftDeleteTransactionAsync(Guid transactionId);
        Task<TransactionSummaryDto> GetTransactionSummaryAsync(CustomerFilterDto filterDto);
    }
}