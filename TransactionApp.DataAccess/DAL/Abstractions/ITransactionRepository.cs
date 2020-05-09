using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionApp.DataAccess.DAL.Entities;
using TransactionApp.DomainModel.Models;

namespace TransactionApp.DataAccess.DAL.Abstractions
{
    public interface ITransactionRepository : IDisposable
    {
        Task<Transaction> GetByIdAsync(int id);

        Task<bool> ExistsAsync(int id);

        void AddAsync(TransactionCreateModel model);
        Task<List<Transaction>> GetAllAsync();

        Task<List<Transaction>> GetFiltered(string currency,
                                                  DateTimeOffset startDate,
                                                  DateTimeOffset endDate,
                                                  string status);
        void SaveChangesAsync();
    }
}