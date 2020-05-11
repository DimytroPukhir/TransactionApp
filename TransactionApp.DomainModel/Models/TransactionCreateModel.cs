using System;

namespace TransactionApp.DomainModel.Models
{
    public class TransactionCreateModel
    {
        public string PublicId { get; set; }
        public decimal? Amount { get; set; }
        public string Code { get; set; }
        public DateTimeOffset? Date { get; set; }
        public string Status { get; set; }
    }
}