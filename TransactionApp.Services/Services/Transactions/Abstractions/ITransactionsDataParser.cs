using System;
using System.IO;
using System.Threading.Tasks;

namespace TransactionApp.Services.Services.Transactions.Abstractions
{

        public interface IPartImportDataParser: IDisposable
        {
           // Task<ImportedPart> ParseAsync(string manufacturerName);

            Task<bool> CanParseDataAsync(Stream data);

            Task SetDataSourceAsync(Stream data);
        }
    }