using PersonalFinanceManagement.API.Database.Entities;
using PersonalFinanceManagement.API.Database.Entities.DTOs.Categories;
using PersonalFinanceManagement.API.Database.Entities.DTOs.SplitTransactions;
using PersonalFinanceManagement.API.Database.Entities.DTOs.Transactions;
using PersonalFinanceManagement.API.Models.Analytics;
using PersonalFinanceManagement.API.Models.Categories;
using PersonalFinanceManagement.API.Models.ExceptionHandling;
using PersonalFinanceManagement.API.Models.Pages;
using PersonalFinanceManagement.API.Models.SortOrders;

namespace PersonalFinanceManagement.API.Database.Repositories
{
    public interface ITransactionRepository
    {
        Task ImportTransactionsFromCSV(CreateTransactionListDTO transactions);

        Task<PagedSortedList<TransactionEntity>> GetTransactions(
            DateTime dateTDate,
            DateTime endDate,
            Kind? transactionKind,
            int? page, 
            int? pageSize,
            string? sortBy, 
            SortOrder? sortOrder);

        Task ImportCategoriesFromCSV(CreateCategoryListDTO categories);

        Task<CategoryList> GetCategories(string parentCode);

        Task<ErrorHandling> CategorizeTransaction(string id, CategorizeDTO categorizeDTO);

        Task<SpendingByCategory> GetAnalytics(DateTime startDate, DateTime endDate, Direction direction, string? catCode);

        Task<ErrorHandling> SplitTransaction(string id, SplitTransactionCommand splitTransactionCommand);
        Task AutoCategorize();
    }
}
