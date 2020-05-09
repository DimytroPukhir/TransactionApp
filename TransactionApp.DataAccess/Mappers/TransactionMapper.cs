using TransactionApp.Common.Mappings.Abstractions;
using TransactionApp.DataAccess.DAL.Entities;
using TransactionApp.DomainModel.Models;

namespace TransactionApp.DataAccess.Mappers
{
    public class TransactionMapper : IMappingProfile<TransactionEntity, Transaction>
    {
        public Transaction Map(TransactionEntity source)
        {
            return new Transaction(source.Id,
                source.Identificator,
                source.Amount,
                source.Code,
                source.Date,
                source.Status);
        }
    }
}