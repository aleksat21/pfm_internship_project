namespace PersonalFinanceManagement.API.Models.Exceptions.DomainExceptions
{
    public class SplitTransactionOverLimitException : BadRequestException
    {
        public SplitTransactionOverLimitException(double limit)
            : base($"Split transactions ammount : {limit} doesn't match the transaction ammount")
        {
        }
    }
}
