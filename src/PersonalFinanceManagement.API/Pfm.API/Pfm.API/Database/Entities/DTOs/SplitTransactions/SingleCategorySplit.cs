using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceManagement.API.Database.Entities.DTOs.SplitTransactions
{
    public class SingleCategorySplit
    {
        [Required]
        public string Catcode { get; set; }

        [Required]
        public double Amount { get; set; }
    }
}
