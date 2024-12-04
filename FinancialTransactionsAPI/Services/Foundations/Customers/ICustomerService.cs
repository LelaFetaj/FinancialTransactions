using FinancialTransactionsAPI.Models.DTOs.Customers;

namespace FinancialTransactionsAPI.Services.Foundations.Customers
{
    public interface ICustomerService
    {
        Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto createCustomerDto);
        Task<CustomerDto> RetrieveCustomerByIdAsync(Guid id);
    }
}