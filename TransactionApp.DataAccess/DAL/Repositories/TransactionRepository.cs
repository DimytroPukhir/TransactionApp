using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using TransactionApp.Common.Mappings.Abstractions;
using TransactionApp.DataAccess.DAL.Entities;
using TransactionApp.DataAccess.DAL.Infrastructure;
using TransactionApp.DataAccess.DAL.Repositories.Abstactions;
using TransactionApp.DomainModel.Models;

namespace TransactionApp.DataAccess.DAL.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ITransactionsContext _context;
        private readonly IMapper _mapper;

        public TransactionRepository(ITransactionsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Transaction> GetByIdAsync(int id)
        {
            var entity = await _context.Transactions.AsNoTracking().AsQueryable().FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<TransactionEntity, Transaction>(entity);
        }

        public Task<bool> ExistsAsync(int id)
        {
            return _context.Transactions.AnyAsync(x => x.Id == id);
        }

        public async Task<List<Transaction>> GetAllAsync()
        {
            var entities = await _context.Transactions.ToListAsync();
            return _mapper.Map<TransactionEntity, Transaction>(entities);
        }

        public async Task<List<Transaction>> GetFiltered(string currency, DateTimeOffset? startDate,
            DateTimeOffset? endDate, string status, string identificator)
        {
            var query = _context.Transactions
                .AsNoTracking()
                .AsQueryable();
            if (!string.IsNullOrEmpty(currency))
            {
                query = query.Where(x => string.Equals(x.Code, currency, StringComparison.InvariantCultureIgnoreCase));
            }

            if (startDate != null && endDate != null)
            {
                query = query.Where(x => x.Date >= startDate && x.Date <= endDate);
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(x => string.Equals(x.Status, status, StringComparison.InvariantCultureIgnoreCase));
            }

            if (!string.IsNullOrEmpty(identificator))
            {
                query = query.Where(x =>
                    string.Equals(x.Identificator, identificator, StringComparison.InvariantCultureIgnoreCase));
            }

            var items = await query.ToListAsync();

            return _mapper.Map<TransactionEntity, Transaction>(items);
        }

        public void AddAsync(TransactionCreateModel model)
        {
            var newTransactionEntity = new TransactionEntity
            {
                Identificator = model.Identificator,
                Code = model.Code, Amount = model.Amount, Date = model.Date, Status = model.Status
            };
            _context.Transactions.Add(newTransactionEntity);
        }

        public void SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}