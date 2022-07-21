namespace PersonalFinanceManagement.API.Models.Pages
{
    public class TransactionWithSplits : Transaction
    {
        public List<SingleTransactionWithSplit>? Splits { get; set; }
    }
}
