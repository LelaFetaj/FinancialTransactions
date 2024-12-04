using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using FinancialTransactionsAPI.Models.DTOs.Customers;

namespace FinancialTransactionsAPI.Services.Foundations.Customers
{
    public class CustomerValidator
    {
        public void ValidateCustomerFilter(CreateCustomerDto createCustomerDto)
        {
            if (createCustomerDto == null)
            {
                throw new ValidationException("Customer data is required.");
            }
        
            if (string.IsNullOrWhiteSpace(createCustomerDto.FullName))
            {
                throw new ValidationException("Customer name cannot be empty.");
            }
        
            if (string.IsNullOrWhiteSpace(createCustomerDto.PhoneNumber))
            {
                throw new ValidationException("Phone number cannot be empty.");
            }
        
            if (!Regex.IsMatch(createCustomerDto.PhoneNumber, @"^\+?[1-9]\d{1,14}$"))
            {
                throw new ValidationException("Invalid phone number format.");
            }
        
            if (string.IsNullOrWhiteSpace(createCustomerDto.Email))
            {
                throw new ValidationException("Email cannot be empty.");
            }
        
            if (!Regex.IsMatch(createCustomerDto.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                throw new ValidationException("Invalid email format.");
            }
        }

        public void ValidateCustomerUpdateFilter(UpdateCustomerDto updateCustomerDto)
        {
            if (updateCustomerDto == null)
            {
                throw new ValidationException("Customer data is required.");
            }

            if (!Regex.IsMatch(updateCustomerDto.PhoneNumber, @"^\+?[1-9]\d{1,14}$"))
            {
                throw new ValidationException("Invalid phone number format.");
            }
        
            if (!Regex.IsMatch(updateCustomerDto.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                throw new ValidationException("Invalid email format.");
            }
        }
    }
}