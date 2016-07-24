namespace Repozytorium.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Towarzystwa1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Fund", new[] { "Towarzystwa_FundTowarzystwoID" });
            RenameColumn(table: "dbo.Fund", name: "Towarzystwa_FundTowarzystwoID", newName: "FundTowarzystwoID");
            AlterColumn("dbo.Fund", "FundTowarzystwoID", c => c.Int(nullable: false));
            CreateIndex("dbo.Fund", "FundTowarzystwoID");
            AlterStoredProcedure(
                "dbo.Fund_Insert",
                p => new
                    {
                        FundName = p.String(),
                        FundTypID = p.Int(),
                        FundTowarzystwoID = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Fund]([FundName], [FundTypID], [FundTowarzystwoID])
                      VALUES (@FundName, @FundTypID, @FundTowarzystwoID)
                      
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
                    },
                body:
                    @"UPDATE [dbo].[Fund]
                      SET [FundName] = @FundName, [FundTypID] = @FundTypID, [FundTowarzystwoID] = @FundTowarzystwoID
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
            DropIndex("dbo.Fund", new[] { "FundTowarzystwoID" });
            AlterColumn("dbo.Fund", "FundTowarzystwoID", c => c.Int());
            RenameColumn(table: "dbo.Fund", name: "FundTowarzystwoID", newName: "Towarzystwa_FundTowarzystwoID");
            CreateIndex("dbo.Fund", "Towarzystwa_FundTowarzystwoID");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
