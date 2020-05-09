using System;

namespace TransactionApp.Models
{
    public class TransactionViewModel
    {
        public TransactionViewModel()
        {
            
        }
        public TransactionViewModel(int id, string identificator, string payment, string status)
        {
            Id = id;
            Identificator = identificator;
            Payment = payment;
            Status = status;
        }

        public int Id { get; }
        public string Identificator { get; }
        public string Payment { get; }
        public string Status { get; }
    }
}