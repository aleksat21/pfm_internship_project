using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.API.Database.Entities;
using PersonalFinanceManagement.API.Database.Entities.DTOs.Categories;
using PersonalFinanceManagement.API.Database.Entities.DTOs.SplitTransactions;
using PersonalFinanceManagement.API.Database.Entities.DTOs.Transactions;
using PersonalFinanceManagement.API.Database.Repositories;
using PersonalFinanceManagement.API.Models;
using PersonalFinanceManagement.API.Services;

namespace PersonalFinanceManagement.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PersonalFinanceManagementController : ControllerBase
    {
        private readonly ILogger<PersonalFinanceManagementController> _logger;
        private readonly ITransactionService _serviceTransactions;

        public PersonalFinanceManagementController(ILogger<PersonalFinanceManagementController> logger, ITransactionService serviceTransactions)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceTransactions = serviceTransactions ?? throw new ArgumentNullException(nameof(serviceTransactions));
        }

        [HttpPost]
        [Route("transactions/import")]
        public async Task<IActionResult> ImportTransactionsCSV([FromBody] CreateTransactionListDTO transactions)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _serviceTransactions.ImportTransactionsFromCSV(transactions);
            
            return Ok();
        }

        [HttpGet]
        [Route("transactions")]
        public async Task<IActionResult> GetTransactions(
            [FromQuery] Kind transactionKind, 
            [FromQuery] DateTime startDate, 
            [FromQuery] DateTime endDate,
            [FromQuery] int? page,
            [FromQuery] int? pageSize,
            [FromQuery] string? sortBy,
            [FromQuery] SortOrder sortOrder)
        {
            page = page ?? 1;
            pageSize = pageSize ?? 10;
            _logger.LogInformation("Returning {page}. page of products", page);

            var result = await _serviceTransactions.GetTransactions(startDate, endDate, transactionKind, page.Value, pageSize.Value, sortBy, sortOrder);
            return Ok(result);
        }

        [HttpPost]
        [Route("categories/import")]
        public async Task<IActionResult> ImportCategoriesCSV([FromBody] CreateCategoryListDTO categories)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _serviceTransactions.ImportCategoriesFromCSV(categories);

            return Ok();
        }

        [HttpGet]
        [Route("categories")]
        public async Task<ActionResult<CategoryList>> GetCategories([FromQuery] string? parentId)
        {

            var result = await _serviceTransactions.GetCategories(parentId);

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("transactions/{id}/categorize")]
        public async Task<IActionResult> CategorizeTransaction([FromRoute] string id, [FromBody] CategorizeDTO category)
        {
            var result = await _serviceTransactions.CategorizeTransaction(id, category);

            if (result == -1)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpGet]
        [Route("spending-analytics")]
        public async Task<ActionResult<SpendingByCategory>> GetAnalytics(
            [FromQuery] string? catCode,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] Direction? direction
        )
        {
            var result = await _serviceTransactions.GetAnalytics(startDate, endDate, direction, catCode);
            return Ok(result);
        }

        [HttpPost]
        [Route("transaction/{id}/split")]
        public async Task<ActionResult<int>> SplitTransaction([FromRoute] string id, [FromBody] SplitTransactionCommand splitTransactionCommand)
        {
            var result = await _serviceTransactions.SplitTransaction(id, splitTransactionCommand);

            if (result == 404)
            {
                return NotFound();
            }

            if (result == 440)
            {
                // TODO FIX ERROR HANDLING
                return BadRequest();
            }

            return Ok();
        }


    }
}
