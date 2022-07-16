using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.API.Entities;

namespace PersonalFinanceManagement.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PersonalFinanceManagementController : ControllerBase
    {
        [HttpPost]
        [Route("transactions/import")]
        public async Task<ActionResult<IEnumerable<Transaction>>> SaveCSV([FromBody]TransactionList transactions)
        {
            return Ok(transactions.Transactions);
        }
    }
}
