namespace PersonalFinanceManagement.API.Database.Entities.DTOs.Transactions
{
    public class CreateTransactionListDTO
    {
        public List<CreateTransactionDTO> Transactions { get; set; } = new List<CreateTransactionDTO>();

    }
}
