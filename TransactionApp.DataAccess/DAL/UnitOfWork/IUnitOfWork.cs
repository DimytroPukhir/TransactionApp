using System.Threading.Tasks;
using TransactionApp.DataAccess.DAL.Infrastructure;
using TransactionApp.DataAccess.DAL.Repositories.Abstactions;

namespace TransactionApp.DataAccess.DAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        ITransactionRepository TransactionRepository { get; }
        void SaveChanges();
    }
}