using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.API.Database.Entities;
using PersonalFinanceManagement.API.Database.Entities.DTOs;
using PersonalFinanceManagement.API.Database.Repositories;
using PersonalFinanceManagement.API.Models;

namespace PersonalFinanceManagement.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PersonalFinanceManagementController : ControllerBase
    {
        private readonly ITransactionRepository _repository;
        private readonly ILogger<PersonalFinanceManagementController> _logger;

        public PersonalFinanceManagementController(ITransactionRepository repository, ILogger<PersonalFinanceManagementController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [Route("transactions/import")]
        public async Task<IActionResult> ImportTransactionsCSV([FromBody] CreateTransactionListDTO transactions)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _repository.ImportTransactionsFromCSV(transactions);
            
            return Ok();
        }

        //public async Task<TransactionDTO> GetTransactionsAsync([FromQuery] string? transactionKind,
        //                                                 [FromQuery] string? startDate,
        //                                                 [FromQuery] string? endDate,
        //                                                 [FromQuery] int? page,
        //                                                 [FromQuery] int? pageSize,
        //                                                 [FromQuery] string sortBy,
        //                                                 [FromQuery] SortOrder sortOrder
        //                                                 )
        //{
        //
        //}


        }
}
