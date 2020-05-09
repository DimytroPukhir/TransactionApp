using System;

namespace TransactionApp.Services.Models
{
    public class Transaction
    {
        public Transaction(int id, string identificator, decimal amount, string code, DateTimeOffset date, string status)
        {
            Id = id;
            Identificator = identificator;
            Amount = amount;
            Code = code;
            Date = date;
            Status = status;
        }
        public int Id { get; }
        public string Identificator { get; }
        public decimal Amount { get; }
        public string Code { get; }
        public DateTimeOffset Date { get; }
        public string Status { get; }
    }
}