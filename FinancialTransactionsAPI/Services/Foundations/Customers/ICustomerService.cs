using FinancialTransactionsAPI.Models.DTOs.Customers;

namespace FinancialTransactionsAPI.Services.Foundations.Customers
{
    public interface ICustomerService
    {
        Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto createCustomerDto);
        Task<CustomerDto> RetrieveCustomerByIdAsync(Guid id);
        Task<List<CustomerDto>> GetAllCustomersAsync();
        Task<CustomerDto> ModifyCustomerAsync(UpdateCustomerDto updateCustomerDto);
        Task RemoveCustomerAsync(Guid customerId);
    }
}