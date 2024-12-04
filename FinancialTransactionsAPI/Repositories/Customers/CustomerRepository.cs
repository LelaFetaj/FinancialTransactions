using FinancialTransactionsAPI.Models.Entities.Customers;
using FinancialTransactionsAPI.Models.Entities.Transactions;
using Microsoft.EntityFrameworkCore;

namespace FinancialTransactionsAPI.Repositories.Customers
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly StorageBroker _storageBroker;
        public CustomerRepository(StorageBroker storageBroker)
        {
            _storageBroker = storageBroker;
        }
        public async Task AddCustomerAsync(Customer customer)
        {
            await _storageBroker.Customers.AddAsync(customer);
            await _storageBroker.SaveChangesAsync();
        }

        public async Task<Customer> GetCustomerByIdAsync(Guid id)
        {
            return await _storageBroker.Customers
                .Where(c => c.CustomerId == id)
                .Select(c => new Customer
                {
                    CustomerId = c.CustomerId,
                    FullName = c.FullName,
                    PhoneNumber = c.PhoneNumber,
                    Address = c.Address,
                    Email = c.Email,
                    Transactions = c.Transactions.Where(t => t.IsActive).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            var customers = await _storageBroker.Customers
                .Include(c => c.Transactions)
                .ToListAsync();

            return customers; 
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            _storageBroker.Customers.Update(customer);
            await _storageBroker.SaveChangesAsync();
        }
        
        public async Task DeleteCustomerAsync(Customer customer)
        {
            _storageBroker.Customers.Remove(customer);
            await _storageBroker.SaveChangesAsync();
        }

        public async Task<Customer?> GetCustomerByEmailOrPhoneAsync(string email, string phoneNumber)
        {
            return await _storageBroker.Customers
                .Where(c => c.Email == email || c.PhoneNumber == phoneNumber)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> HasTransactionsAsync(Guid customerId)
        {
            return await _storageBroker.Transactions
                .AnyAsync(t => t.CustomerId == customerId && t.IsActive);
        }
    }
}