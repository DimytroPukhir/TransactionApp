namespace TransactionApp.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class
        Configuration : DbMigrationsConfiguration<TransactionApp.DataAccess.DAL.Context.TransactionsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TransactionApp.DataAccess.DAL.Context.TransactionsContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}