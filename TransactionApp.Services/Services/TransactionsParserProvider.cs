using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using TransactionApp.Services.Services.Transactions.Abstractions;

namespace TransactionApp.Services.Services
{
    public class TransactionsParserProvider:ITransactionsParserProvider
    { private readonly IComponentContext _componentContext;

        public TransactionsParserProvider(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }
        public async Task<ITransactionsDataParser> GetParserFor(Stream data)
        {
            if (!data.CanSeek)
            {
                throw new ArgumentException(nameof(data));
            }

            var availableParsers = _componentContext.Resolve<IEnumerable<ITransactionsDataParser>>();

            foreach (var parser in availableParsers)
            {
                data.Seek(0, SeekOrigin.Begin);

                if (await parser.CanParseDataAsync(data))
                {
                    data.Seek(0, SeekOrigin.Begin); 
                    parser.SetDataSourceAsync(data);

                    return parser;
                }

                parser.Dispose();
            }

            return null;
        }
    }
}