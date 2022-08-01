using PersonalFinanceManagement.API.Models.Exceptions;

namespace PersonalFinanceManagement.API.Models.ExceptionHandling.Exceptions.DomainExceptions
{
    public class SingleSplitTransactionException : BadRequestException
    {
        public SingleSplitTransactionException() : base($"Splits must be greater 1 one in order to split a transaction")
        {
        }
    }
}
