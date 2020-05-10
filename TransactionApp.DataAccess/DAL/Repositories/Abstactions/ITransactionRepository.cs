using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionApp.DomainModel.Models;

namespace TransactionApp.DataAccess.DAL.Repositories.Abstactions
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetAllAsync();

        Task<List<Transaction>> GetFiltered(string currencyCode,
            DateTimeOffset? startDate,
            DateTimeOffset? endDate,
            string status);

        void AddAsync(TransactionCreateModel model);
    }
}