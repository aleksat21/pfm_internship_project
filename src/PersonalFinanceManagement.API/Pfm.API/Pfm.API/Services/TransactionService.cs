using AutoMapper;
using PersonalFinanceManagement.API.Database.Entities;
using PersonalFinanceManagement.API.Database.Entities.DTOs.Categories;
using PersonalFinanceManagement.API.Database.Entities.DTOs.SplitTransactions;
using PersonalFinanceManagement.API.Database.Entities.DTOs.Transactions;
using PersonalFinanceManagement.API.Database.Repositories;
using PersonalFinanceManagement.API.Models.Analytics;
using PersonalFinanceManagement.API.Models.Categories;
using PersonalFinanceManagement.API.Models.ExceptionHandling;
using PersonalFinanceManagement.API.Models.Exceptions.DomainExceptions;
using PersonalFinanceManagement.API.Models.Pages;
using PersonalFinanceManagement.API.Models.SortOrders;
using System.Linq;

namespace PersonalFinanceManagement.API.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public TransactionService(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<PagedSortedList<TransactionWithSplits>> GetTransactions(
            DateTime startDate,
            DateTime endDate,
            Kind? transactionKind,
            int? page,
            int? pageSize,
            string? sortBy,
            SortOrder? sortOrder
        )
        {
            var result = await _transactionRepository.GetTransactions(startDate, endDate, transactionKind, page, pageSize, sortBy, sortOrder);

            return new PagedSortedList<TransactionWithSplits>
            {
                PageSize = result.PageSize,
                Page = result.Page,
                TotalCount = result.TotalCount,
                SortBy = result.SortBy,
                SortOrder = result.SortOrder,
                Items = _mapper.Map<List<TransactionWithSplits>>(result.Items)
        };
        }

        public async Task ImportTransactionsFromCSV(CreateTransactionListDTO transactions)
        {
            await _transactionRepository.ImportTransactionsFromCSV(transactions);
        }

        public async Task ImportCategoriesFromCSV(CreateCategoryListDTO categories)
        {
            await _transactionRepository.ImportCategoriesFromCSV(categories);
        }

        public async Task<CategoryList> GetCategories(string parentCode)
        {
            return await _transactionRepository.GetCategories(parentCode);
        }

        public async Task CategorizeTransaction(string id, CategorizeDTO categorizeDTO)
        {
            var result = await _transactionRepository.CategorizeTransaction(id, categorizeDTO);
            
            switch (result)
            {
                case ErrorHandling.CATEGORY_DOESNT_EXIST:
                    throw new CategoryNotFoundException(categorizeDTO.Catcode);
                case ErrorHandling.TRANSACTION_DOESNT_EXIST:
                    throw new TransactionNotFoundException(id);
                default:
                    break;
            }
        }

        public async Task<SpendingByCategory> GetAnalytics(DateTime startDate, DateTime endDate, Direction direction, string? catCode)
        {
            return await _transactionRepository.GetAnalytics(startDate, endDate, direction, catCode);
        }

        public async Task SplitTransaction(string id, SplitTransactionCommand splitTransactionCommand)
        {
            var result = await _transactionRepository.SplitTransaction(id, splitTransactionCommand);

            switch (result)
            {
                case ErrorHandling.CATEGORY_DOESNT_EXIST:
                    string categories_response = string.Join(",", splitTransactionCommand.splits.Select(st => st.Catcode));
                    throw new CategoryNotFoundException(categories_response);
                case ErrorHandling.TRANSACTION_DOESNT_EXIST:
                    throw new TransactionNotFoundException(id);
                case ErrorHandling.SPLIT_AMOUNT_OVER_LIMIT:
                    var splitAmount = splitTransactionCommand.splits.Select(s => s.Amount).Sum();
                    var totalAmount = SplitTransactionOverAmountValue.totalTransactionValue;
                    throw new SplitTransactionOverLimitException(splitAmount, totalAmount);
                default:
                    break;
            }
        }

        public async Task AutoCategorize()
        {
            await _transactionRepository.AutoCategorize();
        }
    }
}
