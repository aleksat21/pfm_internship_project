namespace PersonalFinanceManagement.API.Models.Exceptions.DomainExceptions
{
    public class CategoryNotFoundException : NotFoundException
    {
        public CategoryNotFoundException(string id)
            : base($"The category with ID: {id} doesnt' exist in the database")
        {
        }
    }
}
