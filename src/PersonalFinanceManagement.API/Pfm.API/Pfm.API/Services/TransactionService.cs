using AutoMapper;
using PersonalFinanceManagement.API.Database.Entities;
using PersonalFinanceManagement.API.Database.Entities.DTOs;
using PersonalFinanceManagement.API.Database.Repositories;
using PersonalFinanceManagement.API.Models;

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
    }
}
