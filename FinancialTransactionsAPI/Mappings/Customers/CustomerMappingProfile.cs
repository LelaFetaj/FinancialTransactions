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
            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<CreateCustomerDto, Customer>();
            CreateMap<UpdateCustomerDto, Customer>()
                    .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}