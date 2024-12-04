using System.ComponentModel.DataAnnotations;
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
        private readonly ILogger<CustomerService> logger;
        private readonly CustomerValidator customerValidator;

        public CustomerService(
            ICustomerRepository customerRepository, 
            IMapper mapper, 
            ILogger<CustomerService> logger,
            CustomerValidator customerValidator)
        {
            this.customerRepository = customerRepository;
            this.mapper = mapper;
            this.logger = logger;
            this.customerValidator = customerValidator;
        }

        public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto createCustomerDto)
        {
            customerValidator.ValidateCustomerFilter(createCustomerDto);

            var existingCustomer = await customerRepository.GetCustomerByEmailOrPhoneAsync(createCustomerDto.Email, createCustomerDto.PhoneNumber);
            if (existingCustomer != null)
            {
                throw new ValidationException("A customer with the same email or phone number already exists.");
            }

            var customer = mapper.Map<Customer>(createCustomerDto);

            try
            {
                await customerRepository.AddCustomerAsync(customer);
                return mapper.Map<CustomerDto>(customer);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while creating customer.");
                throw new Exception("An error occurred while creating the customer.");
            }
        }

        public async Task<CustomerDto> RetrieveCustomerByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Customer ID cannot be empty.");
            }

            var customer = await customerRepository.GetCustomerByIdAsync(id);

            if (customer == null)
            {
                return null;
            }

            return mapper.Map<CustomerDto>(customer);
        }

        public async Task<List<CustomerDto>> GetAllCustomersAsync()
        {
            var customers = await customerRepository.GetAllCustomersAsync();
            return mapper.Map<List<CustomerDto>>(customers);
        }

        public async Task<CustomerDto> ModifyCustomerAsync(UpdateCustomerDto updateCustomerDto)
        {
            customerValidator.ValidateCustomerUpdateFilter(updateCustomerDto);

            var existingCustomer = await customerRepository.GetCustomerByEmailOrPhoneAsync(updateCustomerDto.Email, updateCustomerDto.PhoneNumber);
            if (existingCustomer != null)
            {
                throw new ValidationException("A customer with the same email or phone number already exists.");
            }

            var customer = await customerRepository.GetCustomerByIdAsync(updateCustomerDto.customerId);
    
            if (customer == null)
            {
                return null;
            }

            if (!string.IsNullOrEmpty(updateCustomerDto.FullName)) 
                customer.FullName = updateCustomerDto.FullName;

            if (!string.IsNullOrEmpty(updateCustomerDto.Address)) 
                customer.Address = updateCustomerDto.Address;

            if (!string.IsNullOrEmpty(updateCustomerDto.Email)) 
                customer.Email = updateCustomerDto.Email;

            if (!string.IsNullOrEmpty(updateCustomerDto.PhoneNumber)) 
                customer.PhoneNumber = updateCustomerDto.PhoneNumber;

            await customerRepository.UpdateCustomerAsync(customer);

            return mapper.Map<CustomerDto>(customer);
        }

        public async Task RemoveCustomerAsync(Guid customerId)
        {
            var customer = await customerRepository.GetCustomerByIdAsync(customerId);
            if (customer == null)
            {
                throw new ValidationException("Customer does not exist");
            }

            var hasTransactions = await customerRepository.HasTransactionsAsync(customerId);
    
            if (hasTransactions)
            {
                throw new ValidationException("The customer cannot be deleted because they have associated transactions.");
            }
            
            await customerRepository.DeleteCustomerAsync(customer);
        }
    }
}