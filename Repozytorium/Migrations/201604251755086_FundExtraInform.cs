namespace Repozytorium.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FundExtraInform : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fund", "FundZarzadzajacy", c => c.String());
            AddColumn("dbo.Fund", "FundDataUruchomienia", c => c.DateTime(nullable: false));
            AddColumn("dbo.Fund", "FundPierwszajWplata", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Fund", "FundNastepnaWplata", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Fund", "FundPolityka", c => c.String());
            AlterStoredProcedure(
                "dbo.Fund_Insert",
                p => new
                    {
                        FundName = p.String(),
                        FundTypID = p.Int(),
                        FundTowarzystwoID = p.Int(),
                        FundWalutaID = p.Int(),
                        FundSymbol = p.String(),
                        FundZarzadzajacy = p.String(),
                        FundDataUruchomienia = p.DateTime(),
                        FundPierwszajWplata = p.Decimal(precision: 18, scale: 2),
                        FundNastepnaWplata = p.Decimal(precision: 18, scale: 2),
                        FundPolityka = p.String(),
                    },
                body:
                    @"INSERT [dbo].[Fund]([FundName], [FundTypID], [FundTowarzystwoID], [FundWalutaID], [FundSymbol], [FundZarzadzajacy], [FundDataUruchomienia], [FundPierwszajWplata], [FundNastepnaWplata], [FundPolityka])
                      VALUES (@FundName, @FundTypID, @FundTowarzystwoID, @FundWalutaID, @FundSymbol, @FundZarzadzajacy, @FundDataUruchomienia, @FundPierwszajWplata, @FundNastepnaWplata, @FundPolityka)
                      
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
                        FundZarzadzajacy = p.String(),
                        FundDataUruchomienia = p.DateTime(),
                        FundPierwszajWplata = p.Decimal(precision: 18, scale: 2),
                        FundNastepnaWplata = p.Decimal(precision: 18, scale: 2),
                        FundPolityka = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[Fund]
                      SET [FundName] = @FundName, [FundTypID] = @FundTypID, [FundTowarzystwoID] = @FundTowarzystwoID, [FundWalutaID] = @FundWalutaID, [FundSymbol] = @FundSymbol, [FundZarzadzajacy] = @FundZarzadzajacy, [FundDataUruchomienia] = @FundDataUruchomienia, [FundPierwszajWplata] = @FundPierwszajWplata, [FundNastepnaWplata] = @FundNastepnaWplata, [FundPolityka] = @FundPolityka
                      WHERE ([FundID] = @FundID)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.Fund", "FundPolityka");
            DropColumn("dbo.Fund", "FundNastepnaWplata");
            DropColumn("dbo.Fund", "FundPierwszajWplata");
            DropColumn("dbo.Fund", "FundDataUruchomienia");
            DropColumn("dbo.Fund", "FundZarzadzajacy");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
