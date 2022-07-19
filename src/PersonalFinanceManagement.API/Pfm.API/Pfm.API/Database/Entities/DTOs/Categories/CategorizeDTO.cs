using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceManagement.API.Database.Entities.DTOs.Categories
{
    public class CategorizeDTO
    {
        [Required]
        public string Catcode { get; set; }
    }
}
