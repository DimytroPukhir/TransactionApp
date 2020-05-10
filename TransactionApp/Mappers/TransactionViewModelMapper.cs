using TransactionApp.Common.Mappings.Abstractions;
using TransactionApp.DomainModel.Models;
using TransactionApp.Models;

namespace TransactionApp.Mappers
{
    public class TransactionViewModelMapper : IMappingProfile<Transaction, TransactionViewModel>
    {
        public TransactionViewModel Map(Transaction source)
        {
            return new TransactionViewModel(source.PublicId, $"{source.Amount}{source.Code}", source.Status);
        }
    }
}