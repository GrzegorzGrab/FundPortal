namespace Repozytorium.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FundFile1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Fund", name: "FundFile_FundFileID", newName: "FundFileID");
            RenameIndex(table: "dbo.Fund", name: "IX_FundFile_FundFileID", newName: "IX_FundFileID");
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
                        FundFileID = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Fund]([FundName], [FundTypID], [FundTowarzystwoID], [FundWalutaID], [FundSymbol], [FundZarzadzajacy], [FundDataUruchomienia], [FundPierwszajWplata], [FundNastepnaWplata], [FundPolityka], [FundFileID])
                      VALUES (@FundName, @FundTypID, @FundTowarzystwoID, @FundWalutaID, @FundSymbol, @FundZarzadzajacy, @FundDataUruchomienia, @FundPierwszajWplata, @FundNastepnaWplata, @FundPolityka, @FundFileID)
                      
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
                        FundFileID = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Fund]
                      SET [FundName] = @FundName, [FundTypID] = @FundTypID, [FundTowarzystwoID] = @FundTowarzystwoID, [FundWalutaID] = @FundWalutaID, [FundSymbol] = @FundSymbol, [FundZarzadzajacy] = @FundZarzadzajacy, [FundDataUruchomienia] = @FundDataUruchomienia, [FundPierwszajWplata] = @FundPierwszajWplata, [FundNastepnaWplata] = @FundNastepnaWplata, [FundPolityka] = @FundPolityka, [FundFileID] = @FundFileID
                      WHERE ([FundID] = @FundID)"
            );
            
            AlterStoredProcedure(
                "dbo.Fund_Delete",
                p => new
                    {
                        FundID = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Fund]
                      WHERE ([FundID] = @FundID)"
            );
            
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Fund", name: "IX_FundFileID", newName: "IX_FundFile_FundFileID");
            RenameColumn(table: "dbo.Fund", name: "FundFileID", newName: "FundFile_FundFileID");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
