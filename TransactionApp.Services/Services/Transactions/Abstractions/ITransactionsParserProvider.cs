using System.IO;
using System.Threading.Tasks;

namespace TransactionApp.Services.Services.Transactions.Abstractions
{
    public interface ITransactionsParserProvider
    {
        Task<ITransactionsDataParser> GetParserFor(Stream data);
    }
}