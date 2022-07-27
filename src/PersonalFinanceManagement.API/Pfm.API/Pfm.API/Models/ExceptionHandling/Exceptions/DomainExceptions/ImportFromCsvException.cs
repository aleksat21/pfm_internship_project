using PersonalFinanceManagement.API.Models.Exceptions;

namespace PersonalFinanceManagement.API.Models.ExceptionHandling.Exceptions.DomainExceptions
{
    public class ImportFromCsvException : BadRequestException
    {
        public ImportFromCsvException(string message) : base(message)
        {
        }
    }
}
