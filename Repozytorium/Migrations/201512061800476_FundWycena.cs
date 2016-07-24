namespace Repozytorium.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FundWycena : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FundWycena",
                c => new
                    {
                        FundWycenaID = c.Int(nullable: false, identity: true),
                        FundWycenaDate = c.DateTime(nullable: false),
                        FundWycenaCena = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FundID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FundWycenaID)
                .ForeignKey("dbo.Fund", t => t.FundID)
                .Index(t => t.FundID);
            
            CreateStoredProcedure(
                "dbo.FundWycena_Insert",
                p => new
                    {
                        FundWycenaDate = p.DateTime(),
                        FundWycenaCena = p.Decimal(precision: 18, scale: 2),
                        FundID = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[FundWycena]([FundWycenaDate], [FundWycenaCena], [FundID])
                      VALUES (@FundWycenaDate, @FundWycenaCena, @FundID)
                      
                      DECLARE @FundWycenaID int
                      SELECT @FundWycenaID = [FundWycenaID]
                      FROM [dbo].[FundWycena]
                      WHERE @@ROWCOUNT > 0 AND [FundWycenaID] = scope_identity()
                      
                      SELECT t0.[FundWycenaID]
                      FROM [dbo].[FundWycena] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[FundWycenaID] = @FundWycenaID"
            );
            
            CreateStoredProcedure(
                "dbo.FundWycena_Update",
                p => new
                    {
                        FundWycenaID = p.Int(),
                        FundWycenaDate = p.DateTime(),
                        FundWycenaCena = p.Decimal(precision: 18, scale: 2),
                        FundID = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[FundWycena]
                      SET [FundWycenaDate] = @FundWycenaDate, [FundWycenaCena] = @FundWycenaCena, [FundID] = @FundID
                      WHERE ([FundWycenaID] = @FundWycenaID)"
            );
            
            CreateStoredProcedure(
                "dbo.FundWycena_Delete",
                p => new
                    {
                        FundWycenaID = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[FundWycena]
                      WHERE ([FundWycenaID] = @FundWycenaID)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.FundWycena_Delete");
            DropStoredProcedure("dbo.FundWycena_Update");
            DropStoredProcedure("dbo.FundWycena_Insert");
            DropForeignKey("dbo.FundWycena", "FundID", "dbo.Fund");
            DropIndex("dbo.FundWycena", new[] { "FundID" });
            DropTable("dbo.FundWycena");
        }
    }
}
