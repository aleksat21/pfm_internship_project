using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PersonalFinanceManagement.API.Database.Entities;
using PersonalFinanceManagement.API.Database.Entities.DTOs.Categories;
using PersonalFinanceManagement.API.Database.Entities.DTOs.SplitTransactions;
using PersonalFinanceManagement.API.Database.Entities.DTOs.Transactions;
using PersonalFinanceManagement.API.Models.Analytics;
using PersonalFinanceManagement.API.Models.AutomaticCategorization;
using PersonalFinanceManagement.API.Models.Categories;
using PersonalFinanceManagement.API.Models.ExceptionHandling;
using PersonalFinanceManagement.API.Models.ExceptionHandling.Exceptions.DomainExceptions;
using PersonalFinanceManagement.API.Models.Pages;
using PersonalFinanceManagement.API.Models.SortOrders;

namespace PersonalFinanceManagement.API.Database.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly TransactionsDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly RulesConfig _rulesConfig;

        public TransactionRepository(TransactionsDbContext dbContext, IMapper mapper, IOptions<RulesConfig> optionsAccessor)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

            if (optionsAccessor == null) throw new ArgumentNullException(nameof(optionsAccessor));
            _rulesConfig = optionsAccessor.Value;
        }

        public async Task ImportTransactionsFromCSV(CreateTransactionListDTO transactions)
        {
            await _dbContext.Transactions.AddRangeAsync(_mapper.Map<IEnumerable<TransactionEntity>>(transactions.Transactions));
            try
            {
                await _dbContext.SaveChangesAsync();

            } catch (Exception e)
            {
                throw new ImportFromCsvException(e.InnerException.Message);
            }
        }
        public async Task<PagedSortedList<TransactionEntity>> GetTransactions(
            DateTime startDate,
            DateTime endDate,
            Kind? transactionKind,
            int? page,
            int? pageSize,
            string? sortBy,
            SortOrder? sortOrder = SortOrder.Asc
        )
        {
            var query = _dbContext.Transactions.Include(t => t.SplitTransactions).AsQueryable();


            if (transactionKind != null)
            {
                query = query.Where(x => x.Kind.Equals(transactionKind));
            }

            if (startDate != DateTime.MinValue)
            {
                query = query.Where(x => x.Date >= startDate);
            }

            if (endDate != DateTime.MinValue)
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
                    case "amount":
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.Amount) : query.OrderByDescending(x => x.Amount);
                        break;
                    default:
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Date);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(x => x.Date);
            }

            var totalCount = query.Count();

            query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);

            var items = await query.ToListAsync();

            return new PagedSortedList<TransactionEntity>
            {
                Page = page.Value,
                PageSize = pageSize.Value,
                TotalCount = totalCount,
                SortBy = sortBy ?? "date",
                SortOrder = sortOrder ?? SortOrder.Asc,
                Items = items,
            };
        }

        public async Task ImportCategoriesFromCSV(CreateCategoryListDTO categories)
        {
            try
            {
                foreach (var category in categories.Categories)
                {
                    var categoryEntity = await _dbContext.Categories.FindAsync(category.Code);

                    if (categoryEntity != null)
                    {
                        categoryEntity.Name = category.Name;

                        if (categoryEntity.ParentCode != null)
                        {
                            categoryEntity.ParentCode = categoryEntity.ParentCode;
                        }

                        _dbContext.Entry(categoryEntity).State = EntityState.Modified;

                    }
                    else
                    {
                        await _dbContext.AddAsync(_mapper.Map<CategoryEntity>(category));
                    }

                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                throw new ImportFromCsvException(e.InnerException.Message);
            }
        }

        public async Task<CategoryList> GetCategories(string parentCode)
        {
            var query = _dbContext.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(parentCode))
            { 
                query = query.Where(x => x.ParentCode == parentCode);
            }

            var items = await query.ToListAsync();
            return new CategoryList
            {
                items = _mapper.Map<List<Category>>(items)
            };
        }

        public async Task<ErrorHandling> CategorizeTransaction(string id, CategorizeDTO categorizeDTO)
        {
            var category = await _dbContext.Categories.FindAsync(categorizeDTO.Catcode);
            if (category == null)
            {
                return ErrorHandling.CATEGORY_DOESNT_EXIST;
            }

            var transaction = await _dbContext.Transactions.Include(x => x.SplitTransactions).Where(t => t.Id == id).FirstOrDefaultAsync();
            if (transaction == null)
            {
                return ErrorHandling.TRANSACTION_DOESNT_EXIST;
            }

            // Nacin da se uklone splitovane transakcije
            if (transaction.SplitTransactions.Count() > 0)
            {
                _dbContext.SplitTransactions.RemoveRange(transaction.SplitTransactions);
                await _dbContext.SaveChangesAsync();
            }

            transaction.Catcode = category.Code;

            _dbContext.Entry(transaction).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return ErrorHandling.OK;
        }

        public async Task<SpendingByCategory> GetAnalytics(DateTime startDate, DateTime endDate, Direction direction, string? catCode)
        {
            var queryCategories = _dbContext.Categories.Include(cat => cat.Transactions).AsQueryable();

            if (catCode != null)
            {
                queryCategories = queryCategories.Where(x => x.Code == catCode);
            }

            var categories = await queryCategories.ToListAsync();

            var spendingByCategory = new SpendingByCategory();

            foreach (var category in categories)
            {
                //Za svaku kategoriju dohvatam i one kategorije cija je ova kategorija roditelj
                var categoryList = await _dbContext.Categories
                    .Where(c => c.ParentCode == category.Code || c.Code == category.Code)
                    .Include(c => c.Transactions)
                    .Include(c => c.SplitTransactions)
                    .ThenInclude(st => st.Transaction)
                    .ToListAsync();

                var amount = 0.0;
                var count = 0;

                foreach(var cat in categoryList)
                {
                    var nonSplittedTransactions = cat.Transactions.Where(t => t.Direction == direction && t.Catcode != "Z");
                    var splittedTransactions = cat.SplitTransactions.Where(st => st.Transaction.Direction == direction);

                    if (!(startDate == DateTime.MinValue))
                    {
                        nonSplittedTransactions = nonSplittedTransactions.Where(t => t.Date >= startDate);
                        splittedTransactions = splittedTransactions.Where(st => st.Transaction.Date >= startDate);
                    }
                    if (!(endDate == DateTime.MinValue))
                    {
                        nonSplittedTransactions = nonSplittedTransactions.Where(t => t.Date <= endDate);
                        splittedTransactions = splittedTransactions.Where(st => st.Transaction.Date <= endDate);
                    }

                    amount += nonSplittedTransactions.Select(t => t.Amount).Sum() + splittedTransactions.Select(t => t.Amount).Sum();
                    count += nonSplittedTransactions.Count() + splittedTransactions.Count();
                }
               
                if (count == 0)
                {
                    continue;
                }

                spendingByCategory.groups.Add(
                    new SpendingInCategory
                    {
                        Catcode = category.Code,
                        Amount = amount,
                        Count = count
                    }
                );
            }
            return spendingByCategory; 
        }

        public async Task<ErrorHandling> SplitTransaction(string id, SplitTransactionCommand splitTransactionCommand)
        {
            var queryTransactions = _dbContext.Transactions.Include(t => t.SplitTransactions).AsQueryable();

            var transaction = queryTransactions.Where(t => t.Id == id).FirstOrDefault();

            if (transaction == null)
            {
                // Transaction Not Found
                return ErrorHandling.TRANSACTION_DOESNT_EXIST;
            }

            if (splitTransactionCommand.splits.Count <= 1)
            {
                return ErrorHandling.SINGLE_SPLIT_TRANSACTION;
            }

            if (splitTransactionCommand.splits.Select(s => s.Amount).Sum() != transaction.Amount)
            {
                // Bussiness logic , transaction over amount
                SplitTransactionOverAmountValue.totalTransactionValue = transaction.Amount;
                return ErrorHandling.SPLIT_AMOUNT_OVER_LIMIT;
            }

            var hasSplits = transaction.SplitTransactions.Count() > 0;

            // HACK
            if (hasSplits)
            {
                var splitsToBeDeleted = await _dbContext.SplitTransactions.Where(st => st.Id == id).ToListAsync();
                _dbContext.SplitTransactions.RemoveRange(splitsToBeDeleted);
                await _dbContext.SaveChangesAsync();
            }
            
 
            transaction.Catcode = "Z";
            _dbContext.Entry(transaction).State = EntityState.Modified;
            

            foreach (var splitTransaction in splitTransactionCommand.splits)
            {
                await _dbContext.AddAsync(new SplitTransactionEntity
                {
                    Id = id,
                    Catcode = splitTransaction.Catcode,
                    Amount = splitTransaction.Amount
                });
            }

            try
            {
                await _dbContext.SaveChangesAsync();
            } catch (DbUpdateException dbException)
            {
                return ErrorHandling.CATEGORY_DOESNT_EXIST;
            }
            

            return ErrorHandling.OK;
        }

        public async Task AutoCategorize()
        {
            foreach (var rule in _rulesConfig.Rules)
            {
                var title = rule.Title;
                var catcode = rule.Catcode;
                var predicate = rule.Predicate;

                var query = $"UPDATE DBO.TRANSACTIONS SET CATCODE = {catcode} WHERE {predicate} AND CATCODE IS NULL";
                await _dbContext.Database.ExecuteSqlRawAsync(query);
            }
        }
    }
}
