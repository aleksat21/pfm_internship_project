using AutoMapper;
using PersonalFinanceManagement.API.Database.Entities;
using PersonalFinanceManagement.API.Database.Entities.DTOs.Categories;
using PersonalFinanceManagement.API.Database.Entities.DTOs.Transactions;
using PersonalFinanceManagement.API.Models.Categories;
using PersonalFinanceManagement.API.Models.Pages;
using PersonalFinanceManagement.API.Extensions;
using System.Linq;

namespace PersonalFinanceManagement.API.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateTransactionDTO, TransactionEntity>().ReverseMap();
            CreateMap<CreateCategoryDTO, CategoryEntity>().ReverseMap();

            CreateMap<PagedSortedList<TransactionEntity>, PagedSortedList<Transaction>>().ReverseMap();

            CreateMap<SplitTransactionEntity, SingleTransactionWithSplit>().ReverseMap();
            CreateMap<TransactionEntity, TransactionWithSplits>().ForMember(
                d => d.Splits, opts => opts.MapFrom(s => s.SplitTransactions.ToList()))
                .AfterMap((src, dest) => dest.Splits = dest.Splits.ToListOrNullIfEmpty()).ReverseMap();

            CreateMap<Transaction, TransactionEntity>().ReverseMap();
            CreateMap<Category, CategoryEntity>().ReverseMap();
        }
    }
}

