using System.Data.Entity;
using TransactionApp.DataAccess.DAL.Context.Abstractions;
using TransactionApp.DataAccess.DAL.Entities;

namespace TransactionApp.DataAccess.DAL.Context
{
    public class TransactionsContext : DbContext, ITransactionsContext
    {
        public TransactionsContext()
            : base("ConnectionString")
        {
        }

        public virtual IDbSet<TransactionEntity> Transactions { get; set; }

        public DbContextTransaction BeginTransaction()
        {
            return Database.BeginTransaction();
        }

        public new void SaveChanges()
        {
            base.SaveChanges();
        }
    }
}