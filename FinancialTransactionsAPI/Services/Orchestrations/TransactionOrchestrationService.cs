using FinancialTransactionsAPI.Models.DTOs.Transactions;
using FinancialTransactionsAPI.Models.Entities.Transactions;
using FinancialTransactionsAPI.Services.Foundations.Transactions;

namespace FinancialTransactionsAPI.Services.Orchestrations
{
    public class TransactionOrchestrationService : ITransactionOrchestrationService
    {
        private readonly ITransactionService transactionService;

        public TransactionOrchestrationService(ITransactionService transactionService)
        {
            this.transactionService = transactionService;
        }

        public async Task<TransactionDto> CreateTransactionAsync(CreateTransactionDto createTransactionDto) =>
            await transactionService.CreateTransactionAsync(createTransactionDto);

        public async Task<List<Transaction>> GetFilteredTransactionsAsync(TransactionFilter filter)
        {
            var transactions = await transactionService.GetAllTransactionsAsync();

            // Filter by TransactionType (enum)
            if (!string.IsNullOrEmpty(filter.TransactionType) && Enum.TryParse(typeof(TransactionType), filter.TransactionType, true, out var transactionType))
            {
                transactions = transactions.Where(t => t.TransactionType == (TransactionType)transactionType).ToList();
            }

            // Filter by TransactionStatus (enum)
            if (!string.IsNullOrEmpty(filter.TransactionStatus) && Enum.TryParse(typeof(Status), filter.TransactionStatus, true, out var status))
            {
                transactions = transactions.Where(t => t.Status == (Status)status).ToList();
            }

            // Filter by TransactionAmount (decimal)
            if (!string.IsNullOrEmpty(filter.TransactionAmount) && decimal.TryParse(filter.TransactionAmount, out var transactionAmount))
            {
                transactions = transactions.Where(t => t.Amount == transactionAmount).ToList();
            }

            // Filter by Customer Full Name
            if (!string.IsNullOrEmpty(filter.CustomerFullName))
            {
                transactions = transactions.Where(t => t.Customer.FullName.Contains(filter.CustomerFullName)).ToList();
            }

            // Filter by Customer Email
            if (!string.IsNullOrEmpty(filter.CustomerEmail))
            {
                transactions = transactions.Where(t => t.Customer.Email.Contains(filter.CustomerEmail)).ToList();
            }

            // Filter by Customer Phone Number
            if (!string.IsNullOrEmpty(filter.CustomerPhoneNumber))
            {
                transactions = transactions.Where(t => t.Customer.PhoneNumber.Contains(filter.CustomerPhoneNumber, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // pagination
            if (filter.PageNumber.HasValue && filter.PageSize.HasValue)
            {
                transactions = transactions.Skip((filter.PageNumber.Value - 1) * filter.PageSize.Value)
                                           .Take(filter.PageSize.Value)
                                           .ToList();
            }

            return transactions;
        }

        public async Task<TransactionDto> GetTransactionByIdAsync(Guid id) =>
            await transactionService.GetTransactionByIdAsync(id);

        public async Task<TransactionDto> ModifyTransactionAsync(UpdateTransactionDto updateTransactionDto) =>
            await transactionService.ModifyTransactionAsync(updateTransactionDto);

        public async Task RemoveTransactionAsync(Guid transactionId) =>
            await transactionService.RemoveTransactionAsync(transactionId);

        public async Task SoftDeleteTransactionAsync(Guid transactionId) =>
            await transactionService.SoftDeleteTransactionAsync(transactionId); 

        public async Task<TransactionSummaryDto> GetTransactionSummaryAsync(CustomerFilterDto filter) =>
            await transactionService.GetTransactionSummaryAsync(filter);

    }
}