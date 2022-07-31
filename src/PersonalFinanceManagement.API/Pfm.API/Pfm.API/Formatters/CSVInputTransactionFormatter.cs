using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text;
using PersonalFinanceManagement.API.Database.Entities;
using CsvHelper;
using System.Globalization;
using PersonalFinanceManagement.API.Database.Entities.DTOs.Transactions;

namespace PersonalFinanceManagement.API.Formatters
{

    public class CSVInputTransactionFormatter : TextInputFormatter
    {
        private static List<char> charsToRemove = new List<char>() { ',' };
        private static string FilterString(string str, List<char> charsToRemove)
        {
            foreach (char c in charsToRemove)
            {
                str = str.Replace(c.ToString(), String.Empty);
            }

            return str;
        }

        public CSVInputTransactionFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/csv"));

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanReadType(Type type)
        {
            if (type == typeof(CreateTransactionListDTO))
            {
                return base.CanReadType(type);
            }
            return false;
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            var request = context.HttpContext.Request;
            using var reader = new StreamReader(request.Body, encoding);


            try
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    CreateTransactionListDTO transactionList = new CreateTransactionListDTO();
                    await csv.ReadAsync();
                    csv.ReadHeader();

                    //id,beneficiary-name,date,direction,amount,description,currency,mcc,kind

                    while (await csv.ReadAsync())
                    {
                        string id = csv.GetField<string>("id").Trim();
                        string beneficiaryName = csv.GetField<string>("beneficiary-name").Trim();
                        string date = csv.GetField<string>("date").Trim();
                        string direction = csv.GetField<string>("direction").Trim();

                        string amount = csv.GetField<string>("amount").Trim();
                        var parserAmount = FilterString(amount, charsToRemove);


                        string description = csv.GetField<string>("description").Trim();
                        string currency = csv.GetField<string>("currency").Trim();
                        string mcc = csv.GetField<string>("mcc").Trim();
                        string kind = csv.GetField<string>("kind").Trim();


                        transactionList.Transactions.Add(new CreateTransactionDTO
                        {
                            Id = id,
                            BeneficiaryName = !String.IsNullOrEmpty(beneficiaryName) ? beneficiaryName : null,
                            Date = DateTime.Parse(date),
                            Direction = Enum.Parse<Direction>(direction),
                            Amount = double.Parse(amount),
                            Description = !String.IsNullOrEmpty(description) ? description: null,
                            Currency = currency,
                            Mcc = !String.IsNullOrEmpty(mcc) ? mcc : null,
                            Kind = Enum.Parse<Kind>(kind.Trim())
                        });
                    }

                    return await InputFormatterResult.SuccessAsync(transactionList);
                }
            }
            catch
            {
                return await InputFormatterResult.FailureAsync();
            }
        }    
    }
}
