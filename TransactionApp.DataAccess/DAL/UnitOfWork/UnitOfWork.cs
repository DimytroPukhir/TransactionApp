using System;
using TransactionApp.DataAccess.DAL.Context.Abstractions;
using TransactionApp.Services.Infrastructure;
using TransactionApp.Services.Infrastructure.Repositories;

namespace TransactionApp.DataAccess.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public ITransactionsContext Context { get; }
        public ITransactionRepository TransactionRepository { get; }

        public UnitOfWork(ITransactionsContext context, ITransactionRepository transactionRepository)
        {
            Context = context;
            TransactionRepository = transactionRepository;
        }

        public void SaveChanges()
        {
                Context.SaveChanges();
        }
    }
}