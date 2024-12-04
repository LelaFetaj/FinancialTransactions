using FinancialTransactionsAPI.Models.Entities.Customers;

namespace FinancialTransactionsAPI.Repositories.Customers
{
    public interface ICustomerRepository
    {
        Task AddCustomerAsync(Customer customer);
        Task<Customer> GetCustomerByIdAsync(Guid id);
    }
}