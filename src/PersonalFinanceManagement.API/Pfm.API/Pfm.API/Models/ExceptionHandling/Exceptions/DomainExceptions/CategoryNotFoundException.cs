namespace PersonalFinanceManagement.API.Models.Exceptions.DomainExceptions
{
    public class CategoryNotFoundException : NotFoundException
    {
        public CategoryNotFoundException(string id)
            : base($"One or more categories in group {id} doesn't exist in the database")
        {
        }
    }
}
