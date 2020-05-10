using System;
using System.Data.Entity;
using TransactionApp.DataAccess.DAL.Entities;
using TransactionApp.DataAccess.DAL.Infrastructure;

namespace TransactionApp.DataAccess.DAL.Context
{
    public class TransactionsContext :DbContext, ITransactionsContext
    {
        public TransactionsContext()
            : base("ConnectionString")
        {
        }

        public virtual IDbSet<TransactionEntity> Transactions { get; set; }

        public void SaveChanges()
        {
            try
            {
                base.SaveChanges();
            }
            catch (Exception ex)
            {

            }

        }
    }
}