using AutoMapper;
using PersonalFinanceManagement.API.Database.Entities;
using PersonalFinanceManagement.API.Database.Entities.DTOs;
using PersonalFinanceManagement.API.Models;

namespace PersonalFinanceManagement.API.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateTransactionDTO, TransactionEntity>().ReverseMap();

            CreateMap<PagedSortedList<TransactionEntity>, PagedSortedList<Transaction>>().ReverseMap();

            CreateMap<Transaction, TransactionEntity>().ReverseMap();
        }
    }
}
