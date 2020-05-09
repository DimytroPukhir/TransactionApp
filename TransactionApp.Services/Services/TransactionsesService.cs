using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using TransactionApp.DataAccess.DAL.Repositories.Abstactions;
using TransactionApp.DataAccess.DAL.UnitOfWork;
using TransactionApp.DomainModel.Models;
using TransactionApp.Services.Abstractions;
using TransactionApp.Services.Services.Transactions.Abstractions;

namespace TransactionApp.Services.Services
{
    public class TransactionsService : ITransactionsService
    {
        private readonly ITransactionsParserProvider _partImportParserProvider;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TransactionsService(ITransactionsParserProvider partImportParserProvider, IUnitOfWork unitOfWork,
            ITransactionRepository transactionRepository)
        {
            _partImportParserProvider = partImportParserProvider;
            _unitOfWork = unitOfWork;
            _transactionRepository = transactionRepository;
        }

        public async Task ProcessTransactionsAsync(Stream data)
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