namespace PersonalFinanceManagement.API.Database.Entities.DTOs.Categories
{
    public class CreateCategoryListDTO
    {
        public List<CreateCategoryDTO> Categories { get; set; } = new List<CreateCategoryDTO>();
    }
}
