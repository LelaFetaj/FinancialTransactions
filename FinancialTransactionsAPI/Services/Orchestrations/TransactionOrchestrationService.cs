using FinancialTransactionsAPI.Models.DTOs.Transactions;
using FinancialTransactionsAPI.Models.Entities.Transactions;
using FinancialTransactionsAPI.Services.Foundations.Transactions;

namespace FinancialTransactionsAPI.Services.Orchestrations
{
    public class TransactionOrchestrationService : ITransactionOrchestrationService
    {
        private readonly ITransactionService transactionService;
        private readonly TransactionValidator transactionValidator;

        public TransactionOrchestrationService(ITransactionService transactionService, TransactionValidator transactionValidator)
        {
            this.transactionService = transactionService;
            this.transactionValidator = transactionValidator;
        }

        public async Task<TransactionDto> CreateTransactionAsync(CreateTransactionDto createTransactionDto) 
        {
            this.transactionValidator.ValidateTransactionData(createTransactionDto);
            return await transactionService.CreateTransactionAsync(createTransactionDto);
        }

        public async Task<List<Transaction>> GetFilteredTransactionsAsync(TransactionFilter filter)
        {
            transactionValidator.ValidateTransactionFilter(filter);
            //customerValidator.ValidateCustomer(filter);

            var transactions = await transactionService.GetAllTransactionsAsync();

            if (filter.TransactionType.HasValue)
            {
                transactions = transactions.Where(t => t.TransactionType == filter.TransactionType.Value).ToList();
            }

            if (filter.TransactionStatus.HasValue)
            {
                transactions = transactions.Where(t => t.Status == filter.TransactionStatus.Value).ToList();
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

        public async Task<TransactionDto> ModifyTransactionAsync(UpdateTransactionDto updateTransactionDto) 
        {
            transactionValidator.ValidateUpdateTransactionFilter(updateTransactionDto);
            return await transactionService.ModifyTransactionAsync(updateTransactionDto);
        }

        public async Task RemoveTransactionAsync(Guid transactionId) =>
            await transactionService.RemoveTransactionAsync(transactionId);

        public async Task SoftDeleteTransactionAsync(Guid transactionId) =>
            await transactionService.SoftDeleteTransactionAsync(transactionId); 

        public async Task<TransactionSummaryDto> GetTransactionSummaryAsync(CustomerFilterDto filter) =>
            await transactionService.GetTransactionSummaryAsync(filter);

    }
}