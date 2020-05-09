using System.Data.Entity;
using TransactionApp.DataAccess.DAL.Entities;

namespace TransactionApp.DataAccess.DAL.Abstractions
{
    public interface ITransactionContext
    {
        IDbSet<TransactionEntity> Transactions { get; set; }
        
    }
}