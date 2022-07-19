﻿using PersonalFinanceManagement.API.Database.Entities;
using PersonalFinanceManagement.API.Database.Entities.DTOs.Categories;
using PersonalFinanceManagement.API.Database.Entities.DTOs.Transactions;
using PersonalFinanceManagement.API.Models;

namespace PersonalFinanceManagement.API.Services
{
    public interface ITransactionService
    {
        Task ImportTransactionsFromCSV(CreateTransactionListDTO transactions);

        Task<PagedSortedList<Transaction>> GetTransactions(
            DateTime dateTDate,
            DateTime endDate,
            Kind transactionKind = Kind.pmt,
            int page = 1,
            int pageSize = 10,
            string sortBy = null,
            SortOrder sortOrder = SortOrder.Asc);

        Task ImportCategoriesFromCSV(CreateCategoryListDTO categories);

        Task<IEnumerable<Category>> GetCategories(string parentCode);

        Task<int> CategorizeTransaction(string id, CategorizeDTO categorizeDTO);

        Task<SpendingByCategory> GetAnalytics(
            DateTime startDate,
            DateTime endDate,
            Direction? direction,
            string? catCode);
    }
}
