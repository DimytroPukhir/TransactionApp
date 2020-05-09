using System.Data.Entity;
using System.Threading.Tasks;
using TransactionApp.DataAccess.DAL.Abstractions;
using TransactionApp.DataAccess.DAL.Entities;

namespace TransactionApp.DataAccess.DAL.Context
{
    public class TransactionContext : DbContext, ITransactionContext
    {
        public TransactionContext()
            : base("ConString")
        {
        }

        public virtual IDbSet<TransactionEntity> Transactions { get; set; }

        public override Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}