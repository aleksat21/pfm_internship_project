﻿using PersonalFinanceManagement.API.Database.Entities;
using PersonalFinanceManagement.API.Database.Entities.DTOs.Categories;
using PersonalFinanceManagement.API.Database.Entities.DTOs.SplitTransactions;
using PersonalFinanceManagement.API.Database.Entities.DTOs.Transactions;
using PersonalFinanceManagement.API.Models;

namespace PersonalFinanceManagement.API.Services
{
    public interface ITransactionService
    {
        Task ImportTransactionsFromCSV(CreateTransactionListDTO transactions);

        Task<PagedSortedList<Transaction>> GetTransactions(
            DateTime startDate,
            DateTime endDate,
            Kind? transactionKind,
            int? page,
            int? pageSize ,
            string? sortBy,
            SortOrder? sortOrder);

        Task ImportCategoriesFromCSV(CreateCategoryListDTO categories);

        Task<CategoryList> GetCategories(string parentCode);

        Task CategorizeTransaction(string id, CategorizeDTO categorizeDTO);

        Task<SpendingByCategory> GetAnalytics(
            DateTime startDate,
            DateTime endDate,
            Direction? direction,
            string? catCode);

        Task SplitTransaction(string id, SplitTransactionCommand splitTransactionCommand);
    }
}
