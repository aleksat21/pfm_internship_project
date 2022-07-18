using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceManagement.API.Database.Entities.DTOs.Categories
{
    public class CreateCategoryDTO
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        public string ParentCode { get; set; }
    }
}
