namespace Repozytorium.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanieSymboluFunduszuDodanieWycenyMinMax : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fund", "FundSymbol", c => c.String(nullable: false));
            AddColumn("dbo.FundWycena", "FundWycenaCenaMin", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.FundWycena", "FundWycenaCenaMax", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterStoredProcedure(
                "dbo.Fund_Insert",
                p => new
                    {
                        FundName = p.String(),
                        FundTypID = p.Int(),
                        FundTowarzystwoID = p.Int(),
                        FundWalutaID = p.Int(),
                        FundSymbol = p.String(),
                    },
                body:
                    @"INSERT [dbo].[Fund]([FundName], [FundTypID], [FundTowarzystwoID], [FundWalutaID], [FundSymbol])
                      VALUES (@FundName, @FundTypID, @FundTowarzystwoID, @FundWalutaID, @FundSymbol)
                      
                      DECLARE @FundID int
                      SELECT @FundID = [FundID]
                      FROM [dbo].[Fund]
                      WHERE @@ROWCOUNT > 0 AND [FundID] = scope_identity()
                      
                      SELECT t0.[FundID]
                      FROM [dbo].[Fund] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[FundID] = @FundID"
            );
            
            AlterStoredProcedure(
                "dbo.Fund_Update",
                p => new
                    {
                        FundID = p.Int(),
                        FundName = p.String(),
                        FundTypID = p.Int(),
                        FundTowarzystwoID = p.Int(),
                        FundWalutaID = p.Int(),
                        FundSymbol = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[Fund]
                      SET [FundName] = @FundName, [FundTypID] = @FundTypID, [FundTowarzystwoID] = @FundTowarzystwoID, [FundWalutaID] = @FundWalutaID, [FundSymbol] = @FundSymbol
                      WHERE ([FundID] = @FundID)"
            );
            
            AlterStoredProcedure(
                "dbo.FundWycena_Insert",
                p => new
                    {
                        FundWycenaDate = p.DateTime(),
                        FundWycenaCena = p.Decimal(precision: 18, scale: 2),
                        FundWycenaCenaMin = p.Decimal(precision: 18, scale: 2),
                        FundWycenaCenaMax = p.Decimal(precision: 18, scale: 2),
                        FundID = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[FundWycena]([FundWycenaDate], [FundWycenaCena], [FundWycenaCenaMin], [FundWycenaCenaMax], [FundID])
                      VALUES (@FundWycenaDate, @FundWycenaCena, @FundWycenaCenaMin, @FundWycenaCenaMax, @FundID)
                      
                      DECLARE @FundWycenaID int
                      SELECT @FundWycenaID = [FundWycenaID]
                      FROM [dbo].[FundWycena]
                      WHERE @@ROWCOUNT > 0 AND [FundWycenaID] = scope_identity()
                      
                      SELECT t0.[FundWycenaID]
                      FROM [dbo].[FundWycena] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[FundWycenaID] = @FundWycenaID"
            );
            
            AlterStoredProcedure(
                "dbo.FundWycena_Update",
                p => new
                    {
                        FundWycenaID = p.Int(),
                        FundWycenaDate = p.DateTime(),
                        FundWycenaCena = p.Decimal(precision: 18, scale: 2),
                        FundWycenaCenaMin = p.Decimal(precision: 18, scale: 2),
                        FundWycenaCenaMax = p.Decimal(precision: 18, scale: 2),
                        FundID = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[FundWycena]
                      SET [FundWycenaDate] = @FundWycenaDate, [FundWycenaCena] = @FundWycenaCena, [FundWycenaCenaMin] = @FundWycenaCenaMin, [FundWycenaCenaMax] = @FundWycenaCenaMax, [FundID] = @FundID
                      WHERE ([FundWycenaID] = @FundWycenaID)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.FundWycena", "FundWycenaCenaMax");
            DropColumn("dbo.FundWycena", "FundWycenaCenaMin");
            DropColumn("dbo.Fund", "FundSymbol");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
