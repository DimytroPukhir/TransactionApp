using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionApp.DomainModel.Models;

namespace TransactionApp.DataAccess.DAL.Repositories.Abstactions
{
    public interface ITransactionRepository
    {
        Task<Transaction> GetByIdAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<List<Transaction>> GetAllAsync();
        Task<List<Transaction>> GetFiltered(string currency,
            DateTimeOffset? startDate,
            DateTimeOffset? endDate,
            string status, string identificator);

        void AddAsync(TransactionCreateModel model);
        void SaveChangesAsync();
    }
}