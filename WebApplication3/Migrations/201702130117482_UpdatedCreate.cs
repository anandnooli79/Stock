namespace WebApplication3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Stocks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SKUId = c.String(nullable: false),
                        StockName = c.String(),
                        StockDescription = c.String(),
                        StockImage = c.String(),
                        StockPrice = c.Double(nullable: false),
                        DiscountPercent = c.Int(nullable: false),
                        IsInStock = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Stocks");
        }
    }
}
