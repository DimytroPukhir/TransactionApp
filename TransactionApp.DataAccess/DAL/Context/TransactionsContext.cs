using System.Data.Entity;
using System.Threading.Tasks;
using TransactionApp.DataAccess.DAL.Entities;
using TransactionApp.DataAccess.DAL.Infrastructure;

namespace TransactionApp.DataAccess.DAL.Context
{
    public class TransactionsContext : DbContext, ITransactionsContext
    {
        public TransactionsContext()
            : base("ConString")
        {
        }

        public virtual IDbSet<TransactionEntity> Transactions { get; set; }
        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}