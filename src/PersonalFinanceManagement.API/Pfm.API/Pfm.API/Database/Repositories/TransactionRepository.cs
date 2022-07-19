using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceManagement.API.Database.Entities;
using PersonalFinanceManagement.API.Database.Entities.DTOs.Categories;
using PersonalFinanceManagement.API.Database.Entities.DTOs.Transactions;
using PersonalFinanceManagement.API.Models;

namespace PersonalFinanceManagement.API.Database.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly TransactionsDbContext _dbContext;
        private readonly IMapper _mapper;

        public TransactionRepository(TransactionsDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task ImportTransactionsFromCSV(CreateTransactionListDTO transactions)
        {
            await _dbContext.Transactions.AddRangeAsync(_mapper.Map<IEnumerable<TransactionEntity>>(transactions.Transactions));
            await _dbContext.SaveChangesAsync();
        }
        public async Task<PagedSortedList<TransactionEntity>> GetTransactions(
            DateTime startDate,
            DateTime endDate,
            Kind transactionKind = Kind.pmt,
            int page = 1,
            int pageSize = 5,
            string sortBy = null,
            SortOrder sortOrder = SortOrder.Asc
        )
        {
            var query = _dbContext.Transactions.AsQueryable();

            var totalCount = query.Count();

            var totalPages = (int)Math.Ceiling(totalCount * 1.0 / pageSize);

            if (transactionKind != null)
            {
                query = query.Where(x => x.Kind.Equals(transactionKind));
            }

            if (!startDate.Equals(DateTime.MinValue))
            {
                query = query.Where(x => x.Date >= startDate);
            }

            if (!endDate.Equals(DateTime.MinValue))
            {
                query = query.Where(x => x.Date <= endDate);
            }

            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "id":
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
                        break;
                    case "beneficiary-name":
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.BeneficiaryName) : query.OrderByDescending(x => x.BeneficiaryName);
                        break;
                    case "date":
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.Date) : query.OrderByDescending(x => x.Date);
                        break;
                    case "direction":
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.Direction) : query.OrderByDescending(x => x.Direction);
                        break;
                    case "description":
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.Description) : query.OrderByDescending(x => x.Description);
                        break;
                    case "currency":
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.Currency) : query.OrderByDescending(x => x.Currency);
                        break;
                    case "mcc":
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.Mcc) : query.OrderByDescending(x => x.Mcc);
                        break;
                    case "kind":
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.Kind) : query.OrderByDescending(x => x.Kind);
                        break;
                    case "catcode":
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.Catcode) : query.OrderByDescending(x => x.Catcode);
                        break;
                    default:
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(x => x.Id);
            }

            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            var items = await query.ToListAsync();

            return new PagedSortedList<TransactionEntity>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Items = items,
                SortBy = sortBy,
                SortOrder = sortOrder
            };
        }

        public async Task ImportCategoriesFromCSV(CreateCategoryListDTO categories)
        {
            foreach (var category in categories.Categories)
            {
                var entity = _mapper.Map<CategoryEntity>(category);
                if (_dbContext.Entry(entity).State == EntityState.Detached)
                {
                    _dbContext.Categories.Add(entity);
                }
                await _dbContext.SaveChangesAsync();
            }
            
        }

        public async Task<IEnumerable<CategoryEntity>> GetCategories(string parentCode)
        {
            var query = _dbContext.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(parentCode))
            { 
                query = query.Where(x => x.ParentCode == parentCode);
            }

            var items = await query.ToListAsync();
            return items;
        }

        public async Task<int> CategorizeTransaction(string id, CategorizeDTO categorizeDTO)
        {
            // Mozda i ne mora ova provera zbog stranog kljuca koji je postavljen, vratiti se kasnije
            var category = await _dbContext.Categories.FindAsync(categorizeDTO.Catcode);
            if (category == null)
            {
                return -1;
            }

            var transaction = await _dbContext.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return -1;
            }

            transaction.Catcode = category.Code;

            _dbContext.Entry(transaction).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return 1;
        }
    }
}
