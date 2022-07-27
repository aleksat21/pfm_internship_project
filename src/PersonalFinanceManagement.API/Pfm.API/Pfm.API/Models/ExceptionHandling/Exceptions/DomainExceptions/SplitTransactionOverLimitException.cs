namespace PersonalFinanceManagement.API.Models.Exceptions.DomainExceptions
{
    public class SplitTransactionOverLimitException : BadRequestException
    {
        public SplitTransactionOverLimitException(double limit, double amount = 0.0)
            : base($"Split transactions ammount : {limit} doesn't match the transaction ammount = {amount}")
        {
        }
    }
}
