using FinancialTransactionsAPI.Models.Entities.Customers;

namespace FinancialTransactionsAPI.Repositories.Customers
{
    public interface ICustomerRepository
    {
        Task AddCustomerAsync(Customer customer);
        Task<Customer> GetCustomerByIdAsync(Guid id);
        Task<List<Customer>> GetAllCustomersAsync();
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(Customer customer);
        Task<Customer?> GetCustomerByEmailOrPhoneAsync(string email, string phoneNumber);
        Task<bool> HasTransactionsAsync(Guid customerId);
    }
}