using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TransactionApp.Common.Constants;
using TransactionApp.DomainModel.Models;
using TransactionApp.Services.Helpers;
using TransactionApp.Services.Services.Transactions.Abstractions;
using TransactionApp.Services.Services.Transactions.Parsers.Models;

namespace TransactionApp.Services.Services.Transactions.Parsers
{
    internal class CsvImportParser : ITransactionsDataParser
    {
        private Dictionary<int, List<string>> _parseErrors= new Dictionary<int, List<string>>();
        private readonly List<string> _statuses = new List<string> {"Approved", "Failed", "Finished"};
        private StreamReader _dataSourceReader;

        public async Task<bool> CanParseDataAsync(Stream stream)
        {
            var reader = new StreamReader(stream);
            var canParse = false;
            try
            {
                var firstLine = await reader.ReadLineAsync();

                if (firstLine != null)
                {
                    firstLine= Regex.Replace(firstLine, @"(?<=\d),(?=\d)", "");
                    var items= firstLine.Split(',').Select(h => h.Trim()).ToList();
                    if (items.Count > 3)
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

        public void SetDataSourceAsync(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentException();
            }

            stream.Seek(0, SeekOrigin.Begin);
            _dataSourceReader = new StreamReader(stream);
        }

        public async Task<ParseResults> ParseAllFileAsync(Stream data)
        {
            var importedItems = new List<TransactionCreateModel>();
            var currentRow = 1;
            var item = await ParseNextRowAsync(currentRow);
            while (item != null)
            {
                importedItems.Add(item);
                item = await ParseNextRowAsync(currentRow);
                currentRow++;
            }

            return new ParseResults(importedItems, _parseErrors);
        }

        public void Dispose()
        {
            _dataSourceReader?.Dispose();
        }

        private async Task<TransactionCreateModel> ParseNextRowAsync(int currentRow)
        {
            if (_dataSourceReader == null)
            {
                throw new ArgumentException();
            }

            if (_dataSourceReader.EndOfStream)
            {
                return null;
            }

            var tryCount = 0;
            string row;

            do
            {
                if (tryCount > 3 || _dataSourceReader.EndOfStream)
                {
                    return null;
                }

                tryCount++;
                row = await _dataSourceReader.ReadLineAsync();
                row = Regex.Replace(row, @"(?<=\d),(?=\d)", "");
            } while (string.IsNullOrWhiteSpace(row));

            var fields = GetFieldsInRowWithPosition(row);

            return GenerateTransactionModel(fields, currentRow);
        }


        private void ParseErrorsLogger(int currentRow, string message)
        {
            if (_parseErrors.ContainsKey(currentRow))
            {
                _parseErrors[currentRow].Add(message);
            }
            else
            {
                _parseErrors.Add(currentRow, new List<string> {message});
            }
        }

        private TransactionCreateModel GenerateTransactionModel(IReadOnlyDictionary<int, string> fields, int currentRow)
        {
            var transaction = new TransactionCreateModel();
            if (fields.ContainsKey(0))
            {
                if (fields[0].Length > 0 && fields[0].Length <= 50)
                {
                    transaction.publicId = fields[0];
                }
                else
                {
                    ParseErrorsLogger(currentRow, ParseErrorsMessagesConstants.LengthError);
                }
            }
            else
            {
                ParseErrorsLogger(currentRow, ParseErrorsMessagesConstants.EmptyItemError);
            }

            if (fields.ContainsKey(1))
            {
                Decimal amount;
                var isDecimal = decimal.TryParse(fields[1], out amount);
                if (isDecimal)
                {
                    transaction.Amount = amount;
                }
                else
                {
                    ParseErrorsLogger(currentRow, ParseErrorsMessagesConstants.TypeError);
                }
            }

            if (fields.ContainsKey(2) && IsAllUpper(fields[2]) && fields[2].Length == 3)
            {
                transaction.Code = fields[2];
            }
            else
            {
                ParseErrorsLogger(currentRow, ParseErrorsMessagesConstants.CodeFormatError);
            }

            if (fields.ContainsKey(3))
            {
                DateTimeOffset date;
                var isDate = DateTimeOffset.TryParseExact(fields[3], "dd/MM/yyyy hh:mm:ss",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out date);

                if (isDate)
                {
                    transaction.Date = date;
                }
                else
                {
                    ParseErrorsLogger(currentRow, ParseErrorsMessagesConstants.DateFormatError);
                }
            }

            if (fields.ContainsKey(4) && _statuses.Contains(fields[4], StringComparer.InvariantCultureIgnoreCase))
            {
                transaction.Status = StatusHelper.GetUnifiedStatus(fields[4]);
            }
            else
            {
                ParseErrorsLogger(currentRow, ParseErrorsMessagesConstants.InappropriateStatusError);
            }

            return transaction;
        }

        private bool IsAllUpper(string input)
        {
            return input.All(t => !char.IsLetter(t) || char.IsUpper(t));
        }

        private static Dictionary<int, string> GetFieldsInRowWithPosition(string line)
        {
            //use substring for deleting quotes or reverse quotes
            return line.Split(',')
                .Select(v => v.Trim())
                .Select((v, i) => new
                {
                    Key = i,
                    Value = v
                })
                .ToDictionary(o => o.Key, o => o.Value.Substring(1, o.Value.Length - 2));
        }
    }
}