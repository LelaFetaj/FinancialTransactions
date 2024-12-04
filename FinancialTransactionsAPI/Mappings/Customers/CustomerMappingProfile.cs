using AutoMapper;
using FinancialTransactionsAPI.Models.DTOs.Customers;
using FinancialTransactionsAPI.Models.Entities.Customers;

namespace FinancialTransactionsAPI.Mappings.Customers
{
    public class CustomerMappingProfile : Profile
    {
        public CustomerMappingProfile()
        {
            //CreateMap<CreateCustomerDTO, Customer>().ReverseMap();
            CreateMap<Customer, CustomerDto>();
            CreateMap<CreateCustomerDto, Customer>();
        }
    }
}