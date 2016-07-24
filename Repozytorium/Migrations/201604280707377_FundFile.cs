namespace Repozytorium.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FundFile : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FundFile",
                c => new
                    {
                        FundFileID = c.Int(nullable: false, identity: true),
                        FundFileName = c.String(),
                        FundType = c.String(),
                        FundContent = c.Binary(),
                    })
                .PrimaryKey(t => t.FundFileID);
            
            AddColumn("dbo.Fund", "FundFile_FundFileID", c => c.Int());
            CreateIndex("dbo.Fund", "FundFile_FundFileID");
            AddForeignKey("dbo.Fund", "FundFile_FundFileID", "dbo.FundFile", "FundFileID");
            CreateStoredProcedure(
                "dbo.FundFile_Insert",
                p => new
                    {
                        FundFileName = p.String(),
                        FundType = p.String(),
                        FundContent = p.Binary(),
                    },
                body:
                    @"INSERT [dbo].[FundFile]([FundFileName], [FundType], [FundContent])
                      VALUES (@FundFileName, @FundType, @FundContent)
                      
                      DECLARE @FundFileID int
                      SELECT @FundFileID = [FundFileID]
                      FROM [dbo].[FundFile]
                      WHERE @@ROWCOUNT > 0 AND [FundFileID] = scope_identity()
                      
                      SELECT t0.[FundFileID]
                      FROM [dbo].[FundFile] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[FundFileID] = @FundFileID"
            );
            
            CreateStoredProcedure(
                "dbo.FundFile_Update",
                p => new
                    {
                        FundFileID = p.Int(),
                        FundFileName = p.String(),
                        FundType = p.String(),
                        FundContent = p.Binary(),
                    },
                body:
                    @"UPDATE [dbo].[FundFile]
                      SET [FundFileName] = @FundFileName, [FundType] = @FundType, [FundContent] = @FundContent
                      WHERE ([FundFileID] = @FundFileID)"
            );
            
            CreateStoredProcedure(
                "dbo.FundFile_Delete",
                p => new
                    {
                        FundFileID = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[FundFile]
                      WHERE ([FundFileID] = @FundFileID)"
            );
            
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
                        FundFile_FundFileID = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Fund]([FundName], [FundTypID], [FundTowarzystwoID], [FundWalutaID], [FundSymbol], [FundZarzadzajacy], [FundDataUruchomienia], [FundPierwszajWplata], [FundNastepnaWplata], [FundPolityka], [FundFile_FundFileID])
                      VALUES (@FundName, @FundTypID, @FundTowarzystwoID, @FundWalutaID, @FundSymbol, @FundZarzadzajacy, @FundDataUruchomienia, @FundPierwszajWplata, @FundNastepnaWplata, @FundPolityka, @FundFile_FundFileID)
                      
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
                        FundFile_FundFileID = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Fund]
                      SET [FundName] = @FundName, [FundTypID] = @FundTypID, [FundTowarzystwoID] = @FundTowarzystwoID, [FundWalutaID] = @FundWalutaID, [FundSymbol] = @FundSymbol, [FundZarzadzajacy] = @FundZarzadzajacy, [FundDataUruchomienia] = @FundDataUruchomienia, [FundPierwszajWplata] = @FundPierwszajWplata, [FundNastepnaWplata] = @FundNastepnaWplata, [FundPolityka] = @FundPolityka, [FundFile_FundFileID] = @FundFile_FundFileID
                      WHERE ([FundID] = @FundID)"
            );
            
            AlterStoredProcedure(
                "dbo.Fund_Delete",
                p => new
                    {
                        FundID = p.Int(),
                        FundFile_FundFileID = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Fund]
                      WHERE (([FundID] = @FundID) AND (([FundFile_FundFileID] = @FundFile_FundFileID) OR ([FundFile_FundFileID] IS NULL AND @FundFile_FundFileID IS NULL)))"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.FundFile_Delete");
            DropStoredProcedure("dbo.FundFile_Update");
            DropStoredProcedure("dbo.FundFile_Insert");
            DropForeignKey("dbo.Fund", "FundFile_FundFileID", "dbo.FundFile");
            DropIndex("dbo.Fund", new[] { "FundFile_FundFileID" });
            DropColumn("dbo.Fund", "FundFile_FundFileID");
            DropTable("dbo.FundFile");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
