namespace PersonalFinanceManagement.API.Models.ExceptionHandling
{
    public static class SplitTransactionOverAmountValue
    {
        public static double totalTransactionValue = 0.0;
    }

    public enum ErrorHandling
    {
        TRANSACTION_DOESNT_EXIST,
        CATEGORY_DOESNT_EXIST,
        SPLIT_AMOUNT_OVER_LIMIT,
        SINGLE_SPLIT_TRANSACTION,
        OK
    }
}
