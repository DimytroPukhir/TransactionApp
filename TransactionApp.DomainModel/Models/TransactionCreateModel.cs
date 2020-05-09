using System;

namespace TransactionApp.DomainModel.Models
{
    public class TransactionCreateModel
    {
        public TransactionCreateModel(string identificator, decimal amount, string code, DateTimeOffset date,
            string status)
        {
            Identificator = identificator;
            Amount = amount;
            Code = code;
            Date = date;
            Status = status;
        }
        public string Identificator { get; }
        public decimal Amount { get; }
        public string Code { get; }
        public DateTimeOffset Date { get; }
        public string Status { get; }
    }
}