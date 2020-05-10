namespace TransactionApp.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitTransactionsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TransactionEntities",
                c => new
                    {
                        Id = c.Guid(nullable: false, defaultValueSql: "newid()"),
                        PublicId = c.String(maxLength: 50),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Code = c.String(maxLength: 3),
                        Date = c.DateTimeOffset(nullable: false, precision: 7),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TransactionEntities");
        }
    }
}
