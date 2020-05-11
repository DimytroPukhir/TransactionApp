using TransactionApp.Services.Infrastructure.Repositories;

namespace TransactionApp.Services.Infrastructure
{
    public interface IUnitOfWork
    {
        ITransactionRepository TransactionRepository { get; }
        void SaveChanges();
    }
}