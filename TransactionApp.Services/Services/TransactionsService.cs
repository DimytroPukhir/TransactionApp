using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentValidation;
using TransactionApp.Common.Exceptions;
using TransactionApp.Services.Abstractions;
using TransactionApp.Services.Infrastructure;
using TransactionApp.Services.Services.Transactions.Abstractions;
using TransactionApp.Services.Validators;

namespace TransactionApp.Services.Services
{
    public class TransactionsService : ITransactionsService
    {
        private readonly ITransactionsParserProvider _transactionsParserProvider;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ParseResultsValidator _validator = new ParseResultsValidator();

        public TransactionsService(ITransactionsParserProvider transactionsParserProvider, IUnitOfWork unitOfWork)
        {
            _transactionsParserProvider = transactionsParserProvider;
            _unitOfWork = unitOfWork;
        }

        public async Task AddTransactionsAsync(Stream data)
        {
            using (var parser = await _transactionsParserProvider.GetParserFor(data))
            {
                if (parser == null)
                {
                    throw new InvalidFileException(new List<string> {"Unknown data format"});
                }
                

                var importedItems = await parser.ParseAllFileAsync();
                
               await _validator.ValidateAndThrowAsync(importedItems);
                foreach (var item in importedItems.Transactions)
                {
                    _unitOfWork.TransactionRepository.AddAsync(item);
                }
                _unitOfWork.SaveChanges();
            }
            
        }
    }
}
