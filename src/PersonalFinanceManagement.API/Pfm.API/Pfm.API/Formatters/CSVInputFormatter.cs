using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text;
using PersonalFinanceManagement.API.Entities;

namespace PersonalFinanceManagement.API.Formatters
{
    public class CSVInputFormatter : TextInputFormatter
    {
        public CSVInputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/csv"));

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanReadType(Type type)
        {
            if (type == typeof(TransactionList))
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
            
            // Remove header
            string line = await reader.ReadLineAsync();

            TransactionList transactionList = new TransactionList();

            // TODO validate INPUT

            while ((line = await reader.ReadLineAsync()) != null)
            {
                var tokens = line.Split(",");

                string id = tokens[0];
                string beneficiaryName = tokens[1];
                string date = tokens[2];
                string direction = tokens[3];
                double amount = 0.0; //double.Parse(tokens[4].Trim());
                string description = tokens[5];
                string currency = tokens[6];
                string mcc = tokens[7];
                string kind = tokens[8];
                string catCode = "0";

                Transaction t = new Transaction()
                {
                    Id = id.Trim(),
                    BeneficiaryName = beneficiaryName.Trim(),
                    Date = DateTime.Parse(date.Trim()),
                    Direction = Enum.Parse<Direction>(direction.Trim()),
                    Amount = amount,
                    Description = description.Trim(),
                    Currency = currency.Trim(),
                    Mcc = mcc.Trim(),
                    Kind = Enum.Parse<Kind>(kind.Trim()),
                    Catcode = catCode
                };

                transactionList.Transactions.Add(t);
            }
           
            return await InputFormatterResult.SuccessAsync(transactionList);
        }
    }
}
