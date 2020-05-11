using System.Data.Entity;
using TransactionApp.DataAccess.DAL.Entities;

namespace TransactionApp.DataAccess.DAL.Context.Abstractions
{
    public interface ITransactionsContext
    {
        IDbSet<TransactionEntity> Transactions { get; }
        DbContextTransaction BeginTransaction();
        void SaveChanges();
    }
}