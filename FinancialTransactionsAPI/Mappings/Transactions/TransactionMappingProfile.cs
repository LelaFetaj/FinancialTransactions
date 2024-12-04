using AutoMapper;
using FinancialTransactionsAPI.Models.DTOs.Transactions;
using FinancialTransactionsAPI.Models.Entities.Transactions;

namespace FinancialTransactionsAPI.Mappings.Transactions
{
    public class TransactionMappingProfile : Profile
    {
        public TransactionMappingProfile()
        {
            //CreateMap<CreateTransactionDTO, Transaction>().ReverseMap();
            CreateMap<Transaction, TransactionDto>();
            CreateMap<CreateTransactionDto, Transaction>().ReverseMap();
            CreateMap<UpdateTransactionDto, Transaction>()
                    .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}