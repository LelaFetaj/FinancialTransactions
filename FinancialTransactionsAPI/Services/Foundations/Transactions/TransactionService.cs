using AutoMapper;
using FinancialTransactionsAPI.Models.DTOs.Transactions;
using FinancialTransactionsAPI.Models.Entities.Transactions;
using FinancialTransactionsAPI.Services.Foundations.Customers;

namespace FinancialTransactionsAPI.Services.Foundations.Transactions
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository transactionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<TransactionService> logger;
        

        public TransactionService(
            ITransactionRepository transactionRepository, 
            IMapper mapper, 
            ILogger<TransactionService> logger)
        {
            this.transactionRepository = transactionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<TransactionDto> CreateTransactionAsync(CreateTransactionDto createTransactionDto)
        {
            var transaction = mapper.Map<Transaction>(createTransactionDto);
            transaction.TransactionCreateDate = DateTimeOffset.UtcNow;

            try
            {
                await transactionRepository.AddTransactionAsync(transaction);
                return mapper.Map<TransactionDto>(transaction);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while creating transaction.");
                throw new Exception("An error occurred while creating the transaction.");
            }
        }

        public async Task<TransactionDto> GetTransactionByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Transaction ID cannot be empty.");
            }

            var transaction = await transactionRepository.GetTransactionByIdAsync(id);

            if (transaction == null || !transaction.IsActive)
            {
                return null;
            }

            return mapper.Map<TransactionDto>(transaction);
        }

        public async Task<List<Transaction>> GetAllTransactionsAsync() =>
            await transactionRepository.GetAllTransactionsAsync();

        public async Task<TransactionDto> ModifyTransactionAsync(UpdateTransactionDto updateTransactionDto)
        {
            var transaction = await transactionRepository.GetTransactionByIdAsync(updateTransactionDto.TransactionId);
    
            if (transaction == null)
            {
                return null;
            }
            
            if (updateTransactionDto.Amount != null) 
                transaction.Amount = (decimal)updateTransactionDto.Amount;

            if (!string.IsNullOrEmpty(updateTransactionDto.Description)) 
                transaction.Description = updateTransactionDto.Description;

            if (updateTransactionDto.TransactionType != null) 
                transaction.TransactionType = (TransactionType)updateTransactionDto.TransactionType;

            if (updateTransactionDto.Status != null) 
                transaction.Status = (Status)updateTransactionDto.Status;

            if (updateTransactionDto.CustomerId != null)
                transaction.CustomerId = (Guid)updateTransactionDto.CustomerId;
            
            transaction.TransactionUpdateDate = DateTimeOffset.UtcNow;
            await transactionRepository.UpdateTransactionAsync(transaction);

            return mapper.Map<TransactionDto>(transaction);
        }

        public async Task RemoveTransactionAsync(Guid transactionId)
        {
            var transaction = await transactionRepository.GetTransactionByIdAsync(transactionId);
            await transactionRepository.DeleteTransactionAsync(transaction);
        }

        public async Task SoftDeleteTransactionAsync(Guid transactionId)
        {
            var transaction = await transactionRepository.GetTransactionByIdAsync(transactionId);
            transaction.IsActive = false;
            await transactionRepository.UpdateTransactionAsync(transaction);
        }

        public async Task<TransactionSummaryDto> GetTransactionSummaryAsync(CustomerFilterDto filterDto)
        {
            var transactions = await transactionRepository.GetFilteredTransactionsAsync(filterDto);

            if (!transactions.Any())
                return null;

            return new TransactionSummaryDto
            {
                TotalTransactions = transactions.Count,
                TotalCredits = transactions.Where(t => t.TransactionType == TransactionType.Credit).Sum(t => t.Amount),
                TotalDebits = transactions.Where(t => t.TransactionType == TransactionType.Debit).Sum(t => t.Amount),
                NetBalance = transactions.Sum(t => t.TransactionType == TransactionType.Credit ? t.Amount : -t.Amount)
            };
        }

    }
}