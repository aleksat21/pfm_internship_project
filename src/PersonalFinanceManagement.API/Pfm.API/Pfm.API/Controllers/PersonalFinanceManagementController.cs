using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.API.Database.Entities;
using PersonalFinanceManagement.API.Database.Entities.DTOs;

namespace PersonalFinanceManagement.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PersonalFinanceManagementController : ControllerBase
    {
        [HttpPost]
        [Route("transactions/import")]
        public async Task<IActionResult> SaveCSV([FromBody] CreateTransactionListDTO transactions)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            return Ok();
        }
    }
}
