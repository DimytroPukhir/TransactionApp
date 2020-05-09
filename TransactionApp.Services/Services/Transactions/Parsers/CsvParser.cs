using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TransactionApp.Common.Mappings.Abstractions;
using TransactionApp.Services.Models;
using TransactionApp.Services.Services.Transactions.Abstractions;

namespace TransactionApp.Services.Services.Transactions.Parsers
{
    internal class CsvImportParser: ITransactionsDataParser
    {
        private readonly IMapper _mapper;
        private StreamReader _dataSourceReader;
        public CsvImportParser(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task<bool> CanParseDataAsync(Stream stream)
        {
            var reader = new StreamReader(stream);
            var canParse = false;
            try
            {
                var firstLine = await reader.ReadLineAsync();

                if (firstLine != null)
                {
                    var foundHeaders = firstLine.Split(';').Select(h => h.Trim()).ToList();

                    if (foundHeaders.Count.Equals(5))
                    {
                        canParse = true;
                    }
                }
            }
            catch
            {
                // ignored
            }

            return  canParse;
        }

        public  void SetDataSourceAsync(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentException();
            }

            stream.Seek(0, SeekOrigin.Begin);
            _dataSourceReader = new StreamReader(stream);
        }

        public async Task<Transaction> ParseNextRowAsync()
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
            }
            while (string.IsNullOrWhiteSpace(row));

            var fields = GetFieldsInRowWithPosition(row);

            return GenerateTransactionModel(fields);
        }

        private Transaction GenerateTransactionModel(Dictionary<int, string> fields)
        {
            return null;
        }

       
        private static Dictionary<int, string> GetFieldsInRowWithPosition(string line)
        {
            return line.Split(';')
                       .Select(v => v.Trim())
                       .Select((v, i) => new
                                         {
                                             Key = i,
                                             Value = v
                                         })
                       .ToDictionary(o => o.Key, o => o.Value);
        }

       

      
        

        

        public void Dispose()
        {
            _dataSourceReader?.Dispose();
        }
    }
}