using System;
using System.Threading.Tasks;
using TransactionApp.DataAccess.DAL.Context;
using TransactionApp.DataAccess.DAL.Infrastructure;
using TransactionApp.DataAccess.DAL.Repositories;
using TransactionApp.DataAccess.DAL.Repositories.Abstactions;

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
            try
            {
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
            }
        }
    }
}