using System.CodeDom;
using System.Collections.Generic;
using TransactionApp.DomainModel.Models;

namespace TransactionApp.Services.Services.Transactions.Parsers.Models
{
    public class ParseResults
    {
        public ParseResults(List<TransactionCreateModel> transactions, Dictionary<int, List<string>> errors)
        {
            Transactions = transactions;
            ParseErrors = errors;
        }

        public List<TransactionCreateModel> Transactions { get;}
        public Dictionary<int, List<string>> ParseErrors { get;}
    }
}