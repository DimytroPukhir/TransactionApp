using System;
using System.IO;
using System.Threading.Tasks;
using TransactionApp.Services.Services.Transactions.Parsers.Models;

namespace TransactionApp.Services.Services.Transactions.Abstractions
{
    public interface ITransactionsDataParser : IDisposable
    {
        Task<ParseResults> ParseAllFileAsync();

        Task<bool> CanParseDataAsync(Stream data);

        void SetDataSourceAsync(Stream data);
    }
}