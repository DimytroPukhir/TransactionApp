using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using TransactionApp.DataAccess.DAL.UnitOfWork;
using TransactionApp.Services.Abstractions;
using TransactionApp.Services.Services.Transactions.Abstractions;

namespace TransactionApp.Services.Services
{
    public class TransactionsService : ITransactionsService
    {
        private readonly ITransactionsParserProvider _partImportParserProvider;
        private readonly IUnitOfWork _unitOfWork;

        public TransactionsService(ITransactionsParserProvider partImportParserProvider, IUnitOfWork unitOfWork)
        {
            _partImportParserProvider = partImportParserProvider;
            _unitOfWork = unitOfWork;
        }

        public async Task ProcessImportAsync(Stream data)
        {
            using (var parser = await _partImportParserProvider.GetParserFor(data))
            {
                if (parser == null)
                {
                    throw new WarningException();
                    //Throw custom exception

                }

                var importedItem = await parser.ParseNextRowAsync();

                while (importedItem != null)
                {
                    importedItem = await parser.ParseNextRowAsync();
                }

                // TODO: implement saving by batches inside loop
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}