namespace PersonalFinanceManagement.API.Database.Entities.DTOs.SplitTransactions
{
    public class SplitTransactionCommand
    {
        public List<SingleCategorySplit> splits { get; set; } = new List<SingleCategorySplit>();
    }
}
