using FinancialTransactionsAPI.Models.Entities.Transactions;

public interface ITransactionRepository
{
    Task AddTransactionAsync(Transaction transaction);
    Task<Transaction> GetTransactionByIdAsync(Guid id);
    Task<List<Transaction>> GetAllTransactionsAsync();
    Task UpdateTransactionAsync(Transaction transaction);
    Task DeleteTransactionAsync(Transaction transaction);
    Task<List<Transaction>> GetFilteredTransactionsAsync(CustomerFilterDto filterDto);
}