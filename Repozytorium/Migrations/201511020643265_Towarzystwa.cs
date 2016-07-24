namespace Repozytorium.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Towarzystwa : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FundTowarzystwo",
                c => new
                    {
                        FundTowarzystwoID = c.Int(nullable: false, identity: true),
                        FundTowarzystwoNazwa = c.String(),
                        FundTowarzystwoDataDodania = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.FundTowarzystwoID);
            
            AddColumn("dbo.Fund", "Towarzystwa_FundTowarzystwoID", c => c.Int());
            CreateIndex("dbo.Fund", "Towarzystwa_FundTowarzystwoID");
            AddForeignKey("dbo.Fund", "Towarzystwa_FundTowarzystwoID", "dbo.FundTowarzystwo", "FundTowarzystwoID");
            CreateStoredProcedure(
                "dbo.FundTowarzystwo_Insert",
                p => new
                    {
                        FundTowarzystwoNazwa = p.String(),
                        FundTowarzystwoDataDodania = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[FundTowarzystwo]([FundTowarzystwoNazwa], [FundTowarzystwoDataDodania])
                      VALUES (@FundTowarzystwoNazwa, @FundTowarzystwoDataDodania)
                      
                      DECLARE @FundTowarzystwoID int
                      SELECT @FundTowarzystwoID = [FundTowarzystwoID]
                      FROM [dbo].[FundTowarzystwo]
                      WHERE @@ROWCOUNT > 0 AND [FundTowarzystwoID] = scope_identity()
                      
                      SELECT t0.[FundTowarzystwoID]
                      FROM [dbo].[FundTowarzystwo] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[FundTowarzystwoID] = @FundTowarzystwoID"
            );
            
            CreateStoredProcedure(
                "dbo.FundTowarzystwo_Update",
                p => new
                    {
                        FundTowarzystwoID = p.Int(),
                        FundTowarzystwoNazwa = p.String(),
                        FundTowarzystwoDataDodania = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[FundTowarzystwo]
                      SET [FundTowarzystwoNazwa] = @FundTowarzystwoNazwa, [FundTowarzystwoDataDodania] = @FundTowarzystwoDataDodania
                      WHERE ([FundTowarzystwoID] = @FundTowarzystwoID)"
            );
            
            CreateStoredProcedure(
                "dbo.FundTowarzystwo_Delete",
                p => new
                    {
                        FundTowarzystwoID = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[FundTowarzystwo]
                      WHERE ([FundTowarzystwoID] = @FundTowarzystwoID)"
            );
            
            AlterStoredProcedure(
                "dbo.Fund_Insert",
                p => new
                    {
                        FundName = p.String(),
                        FundTypID = p.Int(),
                        Towarzystwa_FundTowarzystwoID = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Fund]([FundName], [FundTypID], [Towarzystwa_FundTowarzystwoID])
                      VALUES (@FundName, @FundTypID, @Towarzystwa_FundTowarzystwoID)
                      
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
                        Towarzystwa_FundTowarzystwoID = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Fund]
                      SET [FundName] = @FundName, [FundTypID] = @FundTypID, [Towarzystwa_FundTowarzystwoID] = @Towarzystwa_FundTowarzystwoID
                      WHERE ([FundID] = @FundID)"
            );
            
            AlterStoredProcedure(
                "dbo.Fund_Delete",
                p => new
                    {
                        FundID = p.Int(),
                        Towarzystwa_FundTowarzystwoID = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Fund]
                      WHERE (([FundID] = @FundID) AND (([Towarzystwa_FundTowarzystwoID] = @Towarzystwa_FundTowarzystwoID) OR ([Towarzystwa_FundTowarzystwoID] IS NULL AND @Towarzystwa_FundTowarzystwoID IS NULL)))"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.FundTowarzystwo_Delete");
            DropStoredProcedure("dbo.FundTowarzystwo_Update");
            DropStoredProcedure("dbo.FundTowarzystwo_Insert");
            DropForeignKey("dbo.Fund", "Towarzystwa_FundTowarzystwoID", "dbo.FundTowarzystwo");
            DropIndex("dbo.Fund", new[] { "Towarzystwa_FundTowarzystwoID" });
            DropColumn("dbo.Fund", "Towarzystwa_FundTowarzystwoID");
            DropTable("dbo.FundTowarzystwo");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
