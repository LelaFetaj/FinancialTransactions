using FinancialTransactionsAPI.Models.Entities.Customers;
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
        public async Task<Customer> GetCustomerByIdAsync(Guid id) =>
            await _storageBroker.Customers.Include(t => t.Transactions).FirstOrDefaultAsync(t => t.CustomerId == id);
    }
}