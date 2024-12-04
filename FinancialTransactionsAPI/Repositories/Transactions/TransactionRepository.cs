using System.Linq.Expressions;
using FinancialTransactionsAPI.Models.Entities.Transactions;
using Microsoft.EntityFrameworkCore;

namespace FinancialTransactionsAPI.Repositories.Transactions
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly StorageBroker _storageBroker;

        public TransactionRepository(StorageBroker storageBroker)
        {
            _storageBroker = storageBroker;
        }

        public async Task AddTransactionAsync(Transaction transaction)
        {
            await _storageBroker.Transactions.AddAsync(transaction);
            await _storageBroker.SaveChangesAsync();
        }

        // public async Task<List<Transaction>> GetAllTransactionsAsync() =>
        // await _storageBroker.Transactions.Include(t => t.Customer).ToListAsync();

        public async Task<List<Transaction>> GetAllTransactionsAsync()
        {
            return await _storageBroker.Transactions.Where(t => t.IsActive).Include(t => t.Customer).ToListAsync();
        }

        public async Task<Transaction> GetTransactionByIdAsync(Guid id) =>
            await _storageBroker.Transactions.Include(t => t.Customer).FirstOrDefaultAsync(t => t.TransactionId == id);

        public async Task UpdateTransactionAsync(Transaction transaction)
        {
            _storageBroker.Transactions.Update(transaction);
            await _storageBroker.SaveChangesAsync();
        }

        public async Task DeleteTransactionAsync(Transaction transaction)
        {
            _storageBroker.Transactions.Remove(transaction);
            await _storageBroker.SaveChangesAsync();
        }

        public async Task<List<Transaction>> GetFilteredTransactionsAsync(CustomerFilterDto filterDto)
        {
            var query = _storageBroker.Transactions
                .Where(t => t.IsActive);

            if (filterDto.CustomerId.HasValue)
                query = query.Where(t => t.CustomerId == filterDto.CustomerId.Value);

            if (!string.IsNullOrWhiteSpace(filterDto.PhoneNumber))
                query = query.Where(t => t.Customer.PhoneNumber == filterDto.PhoneNumber);

            if (!string.IsNullOrWhiteSpace(filterDto.Email))
                query = query.Where(t => t.Customer.Email == filterDto.Email);

            if (!string.IsNullOrWhiteSpace(filterDto.FullName))
                query = query.Where(t => t.Customer.FullName.Contains(filterDto.FullName));

            return await query.ToListAsync();
        }

    }
}