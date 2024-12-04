using FinancialTransactionsAPI.Models.DTOs.Transactions;
using FinancialTransactionsAPI.Models.Entities.Transactions;

namespace FinancialTransactionsAPI.Services.Orchestrations
{
    public interface ITransactionOrchestrationService
    {
        Task<TransactionDto> CreateTransactionAsync(CreateTransactionDto createTransactionDTO);
        Task<List<Transaction>> GetFilteredTransactionsAsync(TransactionFilter filter)
;        Task<TransactionDto> GetTransactionByIdAsync(Guid id);
        Task<TransactionDto> ModifyTransactionAsync(UpdateTransactionDto updateTransactionDto);
        Task RemoveTransactionAsync(Guid transactionId);
        Task SoftDeleteTransactionAsync(Guid transactionId);
        Task<TransactionSummaryDto> GetTransactionSummaryAsync(CustomerFilterDto filter);
    }
}