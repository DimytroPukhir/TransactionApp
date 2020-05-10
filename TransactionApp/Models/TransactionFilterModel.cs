using System;

namespace TransactionApp.Models
{
    public class TransactionFilterModel
    {
       public string CurrencyCode { get; set; }
       public string StartDate { get; set; }
       public string EndDate { get; set; }
       public string Status { get; set; }
    }
}