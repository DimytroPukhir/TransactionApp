using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using FluentValidation;
using TransactionApp.DomainModel.Models;
using TransactionApp.Services.Helpers;
using TransactionApp.Services.Services.Transactions.Abstractions;
using TransactionApp.Services.Services.Transactions.Parsers.Models;
using TransactionApp.Services.Validators;

namespace TransactionApp.Services.Services.Transactions.Parsers
{
    internal class XmlTransactionParser : ITransactionsDataParser
    {
        private static string DatePattern => "dd/MM/yyyy hh:mm:ss";
        private Dictionary<int, List<string>> _parseErrors = new Dictionary<int, List<string>>();
        private StreamReader _dataSourceReader;
        public void Dispose()
        {
            _dataSourceReader?.Dispose();
        }

        public async Task<ParseResults> ParseAllFileAsync(Stream data)
        {
            var transactionsDto = DeserializeTransactionsDto();
            var results = new List<TransactionCreateModel>();
            if (transactionsDto != null)
            {
                foreach (var dto in transactionsDto.Transaction)
                {
                    var date = ParseDate(dto);
                    
                    results.Add(new TransactionCreateModel
                    {
                        PublicId = dto.PublicId, Amount = Convert.ToDecimal(dto.PaymentDetails.Amount),
                        Code = dto.PaymentDetails.CurrencyCode, Date = date, Status =StatusHelper.GetUnifiedStatus(dto.Status)
                    });
                }
            }

            return new ParseResults(results, _parseErrors);
        }

        private static DateTimeOffset ParseDate(TransactionItem dto)
        {
            DateTimeOffset.TryParseExact(dto.TransactionDate, DatePattern,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var date);
            return date;
        }
        private static async Task ValidateTransactionDto(XmlTransactionsDto transactionsDto)
        {
            var validator = new TransactionXmlModelValidator();
            await validator.ValidateAndThrowAsync(transactionsDto);
        }

        private XmlTransactionsDto DeserializeTransactionsDto()
        {
            var xmlString = _dataSourceReader.ReadToEnd().Replace("”", "\"").Replace("\r\n", string.Empty);
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
                    throw new Exception(ex.InnerException.Message);
                }
            }

            return transactionsDto;
        }

        public async Task<bool> CanParseDataAsync(Stream stream)
        {
            var canParse = false;
            try
            {
                using (XmlReader reader = XmlReader.Create(stream))
                {
                    if (reader.MoveToContent() == XmlNodeType.Element && string.Equals(reader.Name ,"Transactions",StringComparison.InvariantCultureIgnoreCase))
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