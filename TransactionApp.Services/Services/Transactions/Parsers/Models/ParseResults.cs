using System.Collections.Generic;
using TransactionApp.DomainModel.Models;

namespace TransactionApp.Services.Services.Transactions.Parsers.Models
{
    public class ParseResults
    {
        public ParseResults(List<TransactionCreateModel> transactions)
        {
            Transactions = transactions;
        }

        public List<TransactionCreateModel> Transactions { get;}
    }
}