﻿using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.API.Database.Entities;
using PersonalFinanceManagement.API.Database.Entities.DTOs.Categories;
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
        public async Task<ActionResult<IEnumerable<CreateCategoryDTO>>> ImportCategoriesCSV([FromBody] CreateCategoryListDTO categories)
        {
            return categories.Categories;
        }


    }
}
