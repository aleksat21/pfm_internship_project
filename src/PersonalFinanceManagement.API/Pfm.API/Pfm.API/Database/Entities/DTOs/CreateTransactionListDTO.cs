namespace PersonalFinanceManagement.API.Database.Entities.DTOs
{
    public class CreateTransactionListDTO
    {
        public List<CreateTransactionDTO> Transactions { get; set; } = new List<CreateTransactionDTO>();

    }
}
