using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionApp.DataAccess.DAL.UnitOfWork;
using TransactionApp.DomainModel.Models;
using TransactionApp.Services.Abstractions;

namespace TransactionApp.Services.Providers
{
    public class TransactionsProvider:ITransactionsProvider
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransactionsProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Transaction>> GetAllAsync()
        {
           var items= await  _unitOfWork.TransactionRepository.GetAllAsync();
           return items;
        }

        public Task<List<Transaction>> GetFiltered(string currencyCode, string startDate, string endDate, string status)
        {
            var isValidDateRange = ValidateAndParseDateFilters(startDate, endDate, out var startDateFilter, out var endDateFilter);
            var items = _unitOfWork.TransactionRepository.GetFiltered(currencyCode,isValidDateRange?startDateFilter:(DateTimeOffset?)null , isValidDateRange?endDateFilter:(DateTimeOffset?)null, status);
            return items;
        }

        private static bool ValidateAndParseDateFilters(string startDate, string endDate, out DateTimeOffset startDateFilter,
            out DateTimeOffset endDateFilter)
        {
            var isValidDateRange = false;
            startDateFilter = new DateTimeOffset();
            endDateFilter = new DateTimeOffset();
            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                var hasStartDate = DateTimeOffset.TryParse(startDate, out _);
                var hasEndDate = DateTimeOffset.TryParse(endDate, out _);
                if (hasEndDate && hasStartDate)
                {
                    isValidDateRange = true;

                    DateTimeOffset.TryParse(startDate, out startDateFilter);
                    DateTimeOffset.TryParse(startDate, out endDateFilter);
                }
            }

            return isValidDateRange;
        }
    }
}