using System.IO;
using System.Threading.Tasks;

namespace TransactionApp.Services.Services.Transactions.Abstractions
{
    internal interface ITransactionsParserProvider
    {
        Task<ITransactionsParserProvider> GetParserFor(Stream data);
    }
}