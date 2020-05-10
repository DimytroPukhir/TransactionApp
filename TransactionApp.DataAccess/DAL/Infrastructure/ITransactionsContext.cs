using System.Data.Entity;
using TransactionApp.DataAccess.DAL.Entities;

namespace TransactionApp.DataAccess.DAL.Infrastructure
{
    public interface ITransactionsContext
    {
        IDbSet<TransactionEntity> Transactions { get; set; }
        void SaveChanges();
    }
}