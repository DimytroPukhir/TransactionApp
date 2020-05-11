using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using TransactionApp.Common.Mappings.Abstractions;
using TransactionApp.DataAccess.DAL.Context.Abstractions;
using TransactionApp.DataAccess.DAL.Entities;
using TransactionApp.DomainModel.Models;
using TransactionApp.Services.Infrastructure.Repositories;

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

        public async Task<List<Transaction>> GetAllAsync()
        {
            var entities = await Items.ToListAsync();


            return _mapper.Map<TransactionEntity, Transaction>(entities);
        }

        public async Task<List<Transaction>> GetFiltered(string currencyCode, DateTimeOffset? startDate,
            DateTimeOffset? endDate, string status)
        {
            var query = Items;
            if (!string.IsNullOrEmpty(currencyCode))
            {
                query = query.Where(x =>
                    x.Code.Equals(currencyCode, StringComparison.InvariantCultureIgnoreCase));
            }

            if (startDate != null && endDate != null)
            {
                query = query.Where(x => x.Date >= startDate && x.Date <= endDate);
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(x =>
                    x.Status.Equals(status, StringComparison.InvariantCultureIgnoreCase));
            }

            var items = await query.ToListAsync();
            return _mapper.Map<TransactionEntity, Transaction>(items);
        }

        public void AddAsync(TransactionCreateModel model)
        {
            var newTransactionEntity = new TransactionEntity
            {
                Id = Guid.NewGuid(),
                PublicId = model.PublicId,
                Code = model.Code, Amount = model.Amount.Value, Date = model.Date.Value, Status = model.Status
            };
            using (var transaction = _context.BeginTransaction())
            {
                try
                {
                    _context.Transactions.Add(newTransactionEntity);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
        }

        private IQueryable<TransactionEntity> Items =>
            _context.Transactions.AsNoTracking()
                .AsQueryable();
    }
}