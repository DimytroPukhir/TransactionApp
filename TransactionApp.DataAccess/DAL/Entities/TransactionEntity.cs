using System;
using System.ComponentModel.DataAnnotations;

namespace TransactionApp.DataAccess.DAL.Entities
{
    public class TransactionEntity
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Identificator { get; set; }
        public decimal Amount { get; set; }
        [MinLength(3)] 
        [MaxLength(3)] 
        public string Code { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Status { get; set; }
    }
}