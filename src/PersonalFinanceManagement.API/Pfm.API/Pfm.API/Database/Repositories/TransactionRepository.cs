using AutoMapper;
using PersonalFinanceManagement.API.Database.Entities.DTOs;
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
            _dbContext.Transactions.AddRange(_mapper.Map<IEnumerable<Transaction>>(transactions));
            await _dbContext.SaveChangesAsync();
        }
    }
}
