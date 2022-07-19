namespace PersonalFinanceManagement.API.Models
{
    public class SpendingByCategory
    {
        public List<SpendingInCategory> groups { get; set; } = new List<SpendingInCategory>();

    }
}
