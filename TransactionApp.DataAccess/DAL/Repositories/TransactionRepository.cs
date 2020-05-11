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

        public async Task<List<Transaction>> GetAllAsync()
        {
            var entities = _context.Transactions.ToList();


            return _mapper.Map<TransactionEntity, Transaction>(entities);
        }

        public async Task<List<Transaction>> GetFiltered(string currencyCode, DateTimeOffset? startDate,
            DateTimeOffset? endDate, string status)
        {
            var query = _context.Transactions.AsNoTracking()
                .AsQueryable();
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

            var items = query.ToList();
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
            _context.Transactions.Add(newTransactionEntity);
        }
    }
}