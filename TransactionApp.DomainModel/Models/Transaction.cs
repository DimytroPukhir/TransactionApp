using System;

namespace TransactionApp.DomainModel.Models
{
    public class Transaction
    {
        public Transaction(string publicId, decimal amount, string code, DateTimeOffset date,
            string status)
        {
            PublicId = publicId;
            Amount = amount;
            Code = code;
            Date = date;
            Status = status;
        }

        public string PublicId { get; }
        public decimal Amount { get; }
        public string Code { get; }
        public DateTimeOffset Date { get; }
        public string Status { get; }
    }
}