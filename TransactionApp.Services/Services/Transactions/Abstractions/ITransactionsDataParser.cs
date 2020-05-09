using System;
using System.IO;
using System.Threading.Tasks;
using TransactionApp.Services.Models;

namespace TransactionApp.Services.Services.Transactions.Abstractions
{

        public interface ITransactionsDataParser: IDisposable
        {
            Task<Transaction> ParseNextRowAsync();

            Task<bool> CanParseDataAsync(Stream data);

            void SetDataSourceAsync(Stream data);
        }
    }