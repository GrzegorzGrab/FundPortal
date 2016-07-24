namespace Repozytorium.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FundTowaUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FundTowarzystwo", "FundTowarzystwoAdresUrl", c => c.String());
            AlterStoredProcedure(
                "dbo.FundTowarzystwo_Insert",
                p => new
                    {
                        FundTowarzystwoNazwa = p.String(),
                        FundTowarzystwoAdresUrl = p.String(),
                        FundTowarzystwoDataDodania = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[FundTowarzystwo]([FundTowarzystwoNazwa], [FundTowarzystwoAdresUrl], [FundTowarzystwoDataDodania])
                      VALUES (@FundTowarzystwoNazwa, @FundTowarzystwoAdresUrl, @FundTowarzystwoDataDodania)
                      
                      DECLARE @FundTowarzystwoID int
                      SELECT @FundTowarzystwoID = [FundTowarzystwoID]
                      FROM [dbo].[FundTowarzystwo]
                      WHERE @@ROWCOUNT > 0 AND [FundTowarzystwoID] = scope_identity()
                      
                      SELECT t0.[FundTowarzystwoID]
                      FROM [dbo].[FundTowarzystwo] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[FundTowarzystwoID] = @FundTowarzystwoID"
            );
            
            AlterStoredProcedure(
                "dbo.FundTowarzystwo_Update",
                p => new
                    {
                        FundTowarzystwoID = p.Int(),
                        FundTowarzystwoNazwa = p.String(),
                        FundTowarzystwoAdresUrl = p.String(),
                        FundTowarzystwoDataDodania = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[FundTowarzystwo]
                      SET [FundTowarzystwoNazwa] = @FundTowarzystwoNazwa, [FundTowarzystwoAdresUrl] = @FundTowarzystwoAdresUrl, [FundTowarzystwoDataDodania] = @FundTowarzystwoDataDodania
                      WHERE ([FundTowarzystwoID] = @FundTowarzystwoID)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.FundTowarzystwo", "FundTowarzystwoAdresUrl");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
