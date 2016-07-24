namespace Repozytorium.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FundStoredProc : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                "dbo.Fund_Insert",
                p => new
                    {
                        FundName = p.String(),
                        FundTypID = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Fund]([FundName], [FundTypID])
                      VALUES (@FundName, @FundTypID)
                      
                      DECLARE @FundID int
                      SELECT @FundID = [FundID]
                      FROM [dbo].[Fund]
                      WHERE @@ROWCOUNT > 0 AND [FundID] = scope_identity()
                      
                      SELECT t0.[FundID]
                      FROM [dbo].[Fund] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[FundID] = @FundID"
            );
            
            CreateStoredProcedure(
                "dbo.Fund_Update",
                p => new
                    {
                        FundID = p.Int(),
                        FundName = p.String(),
                        FundTypID = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Fund]
                      SET [FundName] = @FundName, [FundTypID] = @FundTypID
                      WHERE ([FundID] = @FundID)"
            );
            
            CreateStoredProcedure(
                "dbo.Fund_Delete",
                p => new
                    {
                        FundID = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Fund]
                      WHERE ([FundID] = @FundID)"
            );
            
            CreateStoredProcedure(
                "dbo.FundTyp_Insert",
                p => new
                    {
                        FundTypName = p.String(),
                        FundTypOpis = p.String(),
                    },
                body:
                    @"INSERT [dbo].[FundTyp]([FundTypName], [FundTypOpis])
                      VALUES (@FundTypName, @FundTypOpis)
                      
                      DECLARE @FundTypID int
                      SELECT @FundTypID = [FundTypID]
                      FROM [dbo].[FundTyp]
                      WHERE @@ROWCOUNT > 0 AND [FundTypID] = scope_identity()
                      
                      SELECT t0.[FundTypID]
                      FROM [dbo].[FundTyp] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[FundTypID] = @FundTypID"
            );
            
            CreateStoredProcedure(
                "dbo.FundTyp_Update",
                p => new
                    {
                        FundTypID = p.Int(),
                        FundTypName = p.String(),
                        FundTypOpis = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[FundTyp]
                      SET [FundTypName] = @FundTypName, [FundTypOpis] = @FundTypOpis
                      WHERE ([FundTypID] = @FundTypID)"
            );
            
            CreateStoredProcedure(
                "dbo.FundTyp_Delete",
                p => new
                    {
                        FundTypID = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[FundTyp]
                      WHERE ([FundTypID] = @FundTypID)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.FundTyp_Delete");
            DropStoredProcedure("dbo.FundTyp_Update");
            DropStoredProcedure("dbo.FundTyp_Insert");
            DropStoredProcedure("dbo.Fund_Delete");
            DropStoredProcedure("dbo.Fund_Update");
            DropStoredProcedure("dbo.Fund_Insert");
        }
    }
}
