using AutoMapper;
using FinancialTransactionsAPI.Models.DTOs.Customers;
using FinancialTransactionsAPI.Models.Entities.Customers;
using FinancialTransactionsAPI.Repositories.Customers;

namespace FinancialTransactionsAPI.Services.Foundations.Customers
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IMapper mapper;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
        {
            this.customerRepository = customerRepository;
            this.mapper = mapper;
        }

        public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto createCustomerDto)
        {
            var customer = mapper.Map<Customer>(createCustomerDto);
            await customerRepository.AddCustomerAsync(customer);
            return mapper.Map<CustomerDto>(customer);
        }

        public async Task<CustomerDto> RetrieveCustomerByIdAsync(Guid id)
        {
            var customer = await customerRepository.GetCustomerByIdAsync(id);
            return mapper.Map<CustomerDto>(customer);
        }
    }
}