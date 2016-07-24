namespace Repozytorium.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FundTypOplata : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FundOplataTyp",
                c => new
                    {
                        FundOplataTypID = c.Int(nullable: false, identity: true),
                        FundOplataTypNazwa = c.String(nullable: false),
                        FundOplataTypOpis = c.String(),
                        FundOplataTypDataDodania = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.FundOplataTypID);
            
            AddColumn("dbo.FundZakup", "FundOplataTyp_FundOplataTypID", c => c.Int());
            CreateIndex("dbo.FundZakup", "FundOplataTyp_FundOplataTypID");
            AddForeignKey("dbo.FundZakup", "FundOplataTyp_FundOplataTypID", "dbo.FundOplataTyp", "FundOplataTypID");
            CreateStoredProcedure(
                "dbo.FundOplataTyp_Insert",
                p => new
                    {
                        FundOplataTypNazwa = p.String(),
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
            
            CreateStoredProcedure(
                "dbo.FundOplataTyp_Update",
                p => new
                    {
                        FundOplataTypID = p.Int(),
                        FundOplataTypNazwa = p.String(),
                        FundOplataTypOpis = p.String(),
                        FundOplataTypDataDodania = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[FundOplataTyp]
                      SET [FundOplataTypNazwa] = @FundOplataTypNazwa, [FundOplataTypOpis] = @FundOplataTypOpis, [FundOplataTypDataDodania] = @FundOplataTypDataDodania
                      WHERE ([FundOplataTypID] = @FundOplataTypID)"
            );
            
            CreateStoredProcedure(
                "dbo.FundOplataTyp_Delete",
                p => new
                    {
                        FundOplataTypID = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[FundOplataTyp]
                      WHERE ([FundOplataTypID] = @FundOplataTypID)"
            );
            
            AlterStoredProcedure(
                "dbo.FundZakup_Insert",
                p => new
                    {
                        FundZakupIlosc = p.Decimal(precision: 18, scale: 2),
                        FundZakupCena = p.Decimal(precision: 18, scale: 2),
                        FundID = p.Int(),
                        FundZakupDataZakupu = p.DateTime(),
                        UzytkownikID = p.String(maxLength: 128),
                        FundOplataTyp_FundOplataTypID = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[FundZakup]([FundZakupIlosc], [FundZakupCena], [FundID], [FundZakupDataZakupu], [UzytkownikID], [FundOplataTyp_FundOplataTypID])
                      VALUES (@FundZakupIlosc, @FundZakupCena, @FundID, @FundZakupDataZakupu, @UzytkownikID, @FundOplataTyp_FundOplataTypID)
                      
                      DECLARE @FundZakupID int
                      SELECT @FundZakupID = [FundZakupID]
                      FROM [dbo].[FundZakup]
                      WHERE @@ROWCOUNT > 0 AND [FundZakupID] = scope_identity()
                      
                      SELECT t0.[FundZakupID]
                      FROM [dbo].[FundZakup] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[FundZakupID] = @FundZakupID"
            );
            
            AlterStoredProcedure(
                "dbo.FundZakup_Update",
                p => new
                    {
                        FundZakupID = p.Int(),
                        FundZakupIlosc = p.Decimal(precision: 18, scale: 2),
                        FundZakupCena = p.Decimal(precision: 18, scale: 2),
                        FundID = p.Int(),
                        FundZakupDataZakupu = p.DateTime(),
                        UzytkownikID = p.String(maxLength: 128),
                        FundOplataTyp_FundOplataTypID = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[FundZakup]
                      SET [FundZakupIlosc] = @FundZakupIlosc, [FundZakupCena] = @FundZakupCena, [FundID] = @FundID, [FundZakupDataZakupu] = @FundZakupDataZakupu, [UzytkownikID] = @UzytkownikID, [FundOplataTyp_FundOplataTypID] = @FundOplataTyp_FundOplataTypID
                      WHERE ([FundZakupID] = @FundZakupID)"
            );
            
            AlterStoredProcedure(
                "dbo.FundZakup_Delete",
                p => new
                    {
                        FundZakupID = p.Int(),
                        FundOplataTyp_FundOplataTypID = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[FundZakup]
                      WHERE (([FundZakupID] = @FundZakupID) AND (([FundOplataTyp_FundOplataTypID] = @FundOplataTyp_FundOplataTypID) OR ([FundOplataTyp_FundOplataTypID] IS NULL AND @FundOplataTyp_FundOplataTypID IS NULL)))"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.FundOplataTyp_Delete");
            DropStoredProcedure("dbo.FundOplataTyp_Update");
            DropStoredProcedure("dbo.FundOplataTyp_Insert");
            DropForeignKey("dbo.FundZakup", "FundOplataTyp_FundOplataTypID", "dbo.FundOplataTyp");
            DropIndex("dbo.FundZakup", new[] { "FundOplataTyp_FundOplataTypID" });
            DropColumn("dbo.FundZakup", "FundOplataTyp_FundOplataTypID");
            DropTable("dbo.FundOplataTyp");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
