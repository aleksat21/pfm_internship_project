using AutoMapper;
using PersonalFinanceManagement.API.Database.Entities;
using PersonalFinanceManagement.API.Database.Entities.DTOs.Categories;
using PersonalFinanceManagement.API.Database.Entities.DTOs.SplitTransactions;
using PersonalFinanceManagement.API.Database.Entities.DTOs.Transactions;
using PersonalFinanceManagement.API.Database.Repositories;
using PersonalFinanceManagement.API.Models;
using PersonalFinanceManagement.API.Models.Exceptions.DomainExceptions;

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

        public async Task<PagedSortedList<Transaction>> GetTransactions(DateTime dateTDate,
            DateTime endDate,
            Kind transactionKind = Kind.pmt,
            int page = 1,
            int pageSize = 10,
            string sortBy = null,
            SortOrder sortOrder = SortOrder.Asc
        )
        {
            var result = await _transactionRepository.GetTransactions(dateTDate, endDate, transactionKind, page, pageSize, sortBy, sortOrder);
            return _mapper.Map<PagedSortedList<Transaction>>(result);
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
            var result = await _transactionRepository.GetCategories(parentCode);

            if (!result.items.Any())
            {
                return null;
            }

            return result;
        }

        public async Task<int> CategorizeTransaction(string id, CategorizeDTO categorizeDTO)
        {
            var result = await _transactionRepository.CategorizeTransaction(id, categorizeDTO);

            return result;
        }

        public async Task<SpendingByCategory> GetAnalytics(DateTime startDate, DateTime endDate, Direction? direction, string? catCode)
        {
            return await _transactionRepository.GetAnalytics(startDate, endDate, direction, catCode);
        }

        public async Task SplitTransaction(string id, SplitTransactionCommand splitTransactionCommand)
        {
            var result = await _transactionRepository.SplitTransaction(id, splitTransactionCommand);

            switch (result)
            {
                case ErrorHandling.CATEGORY_DOESNT_EXIST:
                    throw new CategoryNotFoundException(null);
                case ErrorHandling.TRANSACTION_DOESNT_EXIST:
                    throw new TransactionNotFoundException(id);
                case ErrorHandling.SPLIT_AMOUNT_OVER_LIMIT:
                    throw new SplitTransactionOverLimitException(splitTransactionCommand.splits.Select(s => s.Amount).Sum());
                default:
                    break;
            }
        }
    }
}
