using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using TransactionApp.Common.Exceptions;
using TransactionApp.Common.Helpers;
using TransactionApp.DomainModel.Models;
using TransactionApp.Services.Services.Transactions.Abstractions;
using TransactionApp.Services.Services.Transactions.Parsers.Models;

namespace TransactionApp.Services.Services.Transactions.Parsers
{
    internal class XmlTransactionParser : ITransactionsDataParser
    {
        private static string DatePattern => "dd/MM/yyyy hh:mm:ss";
        private StreamReader _dataSourceReader;

        public void Dispose()
        {
            _dataSourceReader?.Dispose();
        }

        public async Task<ParseResults> ParseAllFileAsync()
        {
            var transactionsDto = await DeserializeTransactions();
            var results = new List<TransactionCreateModel>();
            if (transactionsDto != null)
            {
                results.AddRange(from dto in transactionsDto.Transaction
                    let date = ParseDate(dto)
                    select new TransactionCreateModel
                    {
                        PublicId = dto.PublicId,
                        Amount = Convert.ToDecimal(dto.PaymentDetails.Amount),
                        Code = dto.PaymentDetails.CurrencyCode,
                        Date = date,
                        Status = StatusHelper.GetUnifiedStatus(dto.Status)
                    });
            }

            return new ParseResults(results);
        }

        private static DateTimeOffset ParseDate(TransactionItem dto)
        {
            DateTimeOffset.TryParseExact(dto.TransactionDate, DatePattern, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var date);
            return date;
        }

        private async Task<XmlTransactionsDto> DeserializeTransactions()
        {
            var xmlString = await _dataSourceReader.ReadToEndAsync();
            xmlString = xmlString.Replace("”", "\"").Replace("\r\n", string.Empty);
            var serializer = new XmlSerializer(typeof(XmlTransactionsDto));
            XmlTransactionsDto transactionsDto;
            using (var reader = new StringReader(xmlString))
            {
                try
                {
                    transactionsDto = (XmlTransactionsDto) serializer.Deserialize(reader);
                }
                catch (Exception ex)
                {
                    throw new InvalidFileException(new List<string> {ex.InnerException?.Message});
                }
            }

            return transactionsDto;
        }

        public async Task<bool> CanParseDataAsync(Stream stream)
        {
            const string rootNodeName = "Transactions";
            var canParse = false;
            try
            {
                using (var reader = XmlReader.Create(stream))
                {
                    if (reader.MoveToContent() == XmlNodeType.Element && string.Equals(reader.Name,
                        rootNodeName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        canParse = true;
                    }
                }
            }
            catch
            {
                // ignored
            }

            return canParse;
        }

        public void SetDataSourceAsync(Stream data)
        {
            if (data == null)
            {
                throw new ArgumentException();
            }

            data.Seek(0, SeekOrigin.Begin);
            _dataSourceReader = new StreamReader(data);
        }
    }
}