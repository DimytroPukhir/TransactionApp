using System.Threading.Tasks;
using TransactionApp.DataAccess.DAL.Infrastructure;
using TransactionApp.DataAccess.DAL.Repositories.Abstactions;

namespace TransactionApp.DataAccess.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private ITransactionsContext _context;
        public ITransactionRepository TransactionRepository { get; }

        public UnitOfWork(ITransactionsContext context, ITransactionRepository transactionRepository)
        {
            _context = context;
            TransactionRepository = transactionRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
           return await _context.SaveChangesAsync();
        }
    }
}