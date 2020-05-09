using System.IO;
using System.Threading.Tasks;
using System.Xml;
using TransactionApp.Common.Mappings.Abstractions;
using TransactionApp.Services.Models;
using TransactionApp.Services.Services.Transactions.Abstractions;

namespace TransactionApp.Services.Services.Transactions.Parsers
{
    public class XmlParser:ITransactionsDataParser
    {
        private readonly IMapper _mapper;
        private StreamReader _dataSourceReader;

        public XmlParser(IMapper mapper, StreamReader dataSourceReader)
        {
            _mapper = mapper;
            _dataSourceReader = dataSourceReader;
        }

        public void Dispose()
        {
            _dataSourceReader?.Dispose();
        }

        public Task<Transaction> ParseNextRowAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> CanParseDataAsync(Stream stream)
        {
            var canParse = false;
            try
            {
                using (XmlReader reader = XmlReader.Create(stream))
                {
                    if (reader.MoveToContent() == XmlNodeType.Element && reader.Name == "DiagReport")
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

        public void SetDataSourceAsync(Stream data)
        {
            throw new System.NotImplementedException();
        }
    }
}