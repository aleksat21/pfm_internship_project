using PersonalFinanceManagement.API.Database.Entities;
using PersonalFinanceManagement.API.Database.Entities.DTOs;
using PersonalFinanceManagement.API.Models;

namespace PersonalFinanceManagement.API.Database.Repositories
{
    public interface ITransactionRepository
    {
        Task ImportTransactionsFromCSV(CreateTransactionListDTO transactions);

        Task<PagedSortedList<Transaction>> GetTransactions(
            DateTime dateTDate,
            DateTime endDate,
            Kind transactionKind = Kind.inc,
            int page = 1, 
            int pageSize = 10,
            string sortBy = null, 
            SortOrder sortOrder = SortOrder.Asc);
    }
}
