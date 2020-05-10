using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TransactionApp.DomainModel.Models;
using TransactionApp.Services.Services.Transactions.Parsers.Models;

namespace TransactionApp.Services.Services.Transactions.Abstractions
{
    public interface ITransactionsDataParser : IDisposable
    {
        Task<ParseResults> ParseAllFileAsync(Stream data);

        Task<bool> CanParseDataAsync(Stream data);

        void SetDataSourceAsync(Stream data);
    }
}