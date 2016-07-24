namespace Repozytorium.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FundWaluta : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FundWaluta",
                c => new
                    {
                        FundWalutaID = c.Int(nullable: false, identity: true),
                        FundWalutaKod = c.String(maxLength: 3),
                        FundWalutaPelnaNazwa = c.String(),
                        FundWalutaDataDodania = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.FundWalutaID)
                .Index(t => t.FundWalutaKod, unique: true);
            
            AddColumn("dbo.Fund", "FundWalutaID", c => c.Int(nullable: false));
            CreateIndex("dbo.Fund", "FundWalutaID");
            AddForeignKey("dbo.Fund", "FundWalutaID", "dbo.FundWaluta", "FundWalutaID");
            CreateStoredProcedure(
                "dbo.FundWaluta_Insert",
                p => new
                    {
                        FundWalutaKod = p.String(maxLength: 3),
                        FundWalutaPelnaNazwa = p.String(),
                        FundWalutaDataDodania = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[FundWaluta]([FundWalutaKod], [FundWalutaPelnaNazwa], [FundWalutaDataDodania])
                      VALUES (@FundWalutaKod, @FundWalutaPelnaNazwa, SYSDATETIME())
                      
                      DECLARE @FundWalutaID int
                      SELECT @FundWalutaID = [FundWalutaID]
                      FROM [dbo].[FundWaluta]
                      WHERE @@ROWCOUNT > 0 AND [FundWalutaID] = scope_identity()
                      
                      SELECT t0.[FundWalutaID]
                      FROM [dbo].[FundWaluta] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[FundWalutaID] = @FundWalutaID"
            );
            
            CreateStoredProcedure(
                "dbo.FundWaluta_Update",
                p => new
                    {
                        FundWalutaID = p.Int(),
                        FundWalutaKod = p.String(maxLength: 3),
                        FundWalutaPelnaNazwa = p.String(),
                        FundWalutaDataDodania = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[FundWaluta]
                      SET [FundWalutaKod] = @FundWalutaKod, [FundWalutaPelnaNazwa] = @FundWalutaPelnaNazwa, [FundWalutaDataDodania] = @FundWalutaDataDodania
                      WHERE ([FundWalutaID] = @FundWalutaID)"
            );
            
            CreateStoredProcedure(
                "dbo.FundWaluta_Delete",
                p => new
                    {
                        FundWalutaID = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[FundWaluta]
                      WHERE ([FundWalutaID] = @FundWalutaID)"
            );
            
            AlterStoredProcedure(
                "dbo.Fund_Insert",
                p => new
                    {
                        FundName = p.String(),
                        FundTypID = p.Int(),
                        FundTowarzystwoID = p.Int(),
                        FundWalutaID = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Fund]([FundName], [FundTypID], [FundTowarzystwoID], [FundWalutaID])
                      VALUES (@FundName, @FundTypID, @FundTowarzystwoID, @FundWalutaID)
                      
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
                    },
                body:
                    @"UPDATE [dbo].[Fund]
                      SET [FundName] = @FundName, [FundTypID] = @FundTypID, [FundTowarzystwoID] = @FundTowarzystwoID, [FundWalutaID] = @FundWalutaID
                      WHERE ([FundID] = @FundID)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.FundWaluta_Delete");
            DropStoredProcedure("dbo.FundWaluta_Update");
            DropStoredProcedure("dbo.FundWaluta_Insert");
            DropForeignKey("dbo.Fund", "FundWalutaID", "dbo.FundWaluta");
            DropIndex("dbo.FundWaluta", new[] { "FundWalutaKod" });
            DropIndex("dbo.Fund", new[] { "FundWalutaID" });
            DropColumn("dbo.Fund", "FundWalutaID");
            DropTable("dbo.FundWaluta");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
