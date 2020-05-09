using System.Data.Entity;
using System.Threading.Tasks;
using TransactionApp.DataAccess.DAL.Entities;

namespace TransactionApp.DataAccess.DAL.Infrastructure
{
    public interface ITransactionsContext
    {
        IDbSet<TransactionEntity> Transactions { get; set; }
        Task<int> SaveChangesAsync();

    }
}