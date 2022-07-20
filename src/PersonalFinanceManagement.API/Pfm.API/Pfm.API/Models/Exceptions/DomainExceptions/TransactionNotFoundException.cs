namespace PersonalFinanceManagement.API.Models.Exceptions.DomainExceptions
{
    public class TransactionNotFoundException : NotFoundException
    {
        public TransactionNotFoundException(string id)
            : base($"The transaction with ID: {id} doesn't exist in the database.")
        {
        }
    }
}
