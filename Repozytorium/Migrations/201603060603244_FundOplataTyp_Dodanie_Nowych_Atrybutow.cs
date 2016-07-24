namespace Repozytorium.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FundOplataTyp_Dodanie_Nowych_Atrybutow : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.FundOplataTyp", new[] { "FundOplataTypNazwa" });
            AlterColumn("dbo.FundOplataTyp", "FundOplataTypNazwa", c => c.String(nullable: false, maxLength: 100));
            CreateIndex("dbo.FundOplataTyp", "FundOplataTypNazwa", unique: true);
            AlterStoredProcedure(
                "dbo.FundOplataTyp_Insert",
                p => new
                    {
                        FundOplataTypNazwa = p.String(maxLength: 100),
                        FundOplataTypOpis = p.String(),
                        FundOplataTypDataDodania = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[FundOplataTyp]([FundOplataTypNazwa], [FundOplataTypOpis], [FundOplataTypDataDodania])
                      VALUES (@FundOplataTypNazwa, @FundOplataTypOpis, @FundOplataTypDataDodania)
                      
                      DECLARE @FundOplataTypID int
                      SELECT @FundOplataTypID = [FundOplataTypID]
                      FROM [dbo].[FundOplataTyp]
                      WHERE @@ROWCOUNT > 0 AND [FundOplataTypID] = scope_identity()
                      
                      SELECT t0.[FundOplataTypID]
                      FROM [dbo].[FundOplataTyp] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[FundOplataTypID] = @FundOplataTypID"
            );
            
            AlterStoredProcedure(
                "dbo.FundOplataTyp_Update",
                p => new
                    {
                        FundOplataTypID = p.Int(),
                        FundOplataTypNazwa = p.String(maxLength: 100),
                        FundOplataTypOpis = p.String(),
                        FundOplataTypDataDodania = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[FundOplataTyp]
                      SET [FundOplataTypNazwa] = @FundOplataTypNazwa, [FundOplataTypOpis] = @FundOplataTypOpis, [FundOplataTypDataDodania] = @FundOplataTypDataDodania
                      WHERE ([FundOplataTypID] = @FundOplataTypID)"
            );
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.FundOplataTyp", new[] { "FundOplataTypNazwa" });
            AlterColumn("dbo.FundOplataTyp", "FundOplataTypNazwa", c => c.String(nullable: false));
            CreateIndex("dbo.FundOplataTyp", "FundOplataTypNazwa", unique: true);
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
