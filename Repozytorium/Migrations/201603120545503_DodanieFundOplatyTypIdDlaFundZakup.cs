namespace Repozytorium.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanieFundOplatyTypIdDlaFundZakup : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.FundZakup", new[] { "FundOplataTyp_FundOplataTypID" });
            RenameColumn(table: "dbo.FundZakup", name: "FundOplataTyp_FundOplataTypID", newName: "FundOplataTypID");
            AlterColumn("dbo.FundZakup", "FundOplataTypID", c => c.Int(nullable: false));
            CreateIndex("dbo.FundZakup", "FundOplataTypID");
            AlterStoredProcedure(
                "dbo.FundZakup_Insert",
                p => new
                    {
                        FundZakupIlosc = p.Decimal(precision: 18, scale: 2),
                        FundZakupCena = p.Decimal(precision: 18, scale: 2),
                        FundID = p.Int(),
                        FundZakupDataZakupu = p.DateTime(),
                        UzytkownikID = p.String(maxLength: 128),
                        FundZakupDataDodaniaZakupu = p.DateTime(),
                        FundOplataTypID = p.Int(),
                        FundOplataWysokosc = p.Decimal(precision: 18, scale: 2),
                    },
                body:
                    @"INSERT [dbo].[FundZakup]([FundZakupIlosc], [FundZakupCena], [FundID], [FundZakupDataZakupu], [UzytkownikID], [FundZakupDataDodaniaZakupu], [FundOplataTypID], [FundOplataWysokosc])
                      VALUES (@FundZakupIlosc, @FundZakupCena, @FundID, @FundZakupDataZakupu, @UzytkownikID, @FundZakupDataDodaniaZakupu, @FundOplataTypID, @FundOplataWysokosc)
                      
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
                        FundZakupDataDodaniaZakupu = p.DateTime(),
                        FundOplataTypID = p.Int(),
                        FundOplataWysokosc = p.Decimal(precision: 18, scale: 2),
                    },
                body:
                    @"UPDATE [dbo].[FundZakup]
                      SET [FundZakupIlosc] = @FundZakupIlosc, [FundZakupCena] = @FundZakupCena, [FundID] = @FundID, [FundZakupDataZakupu] = @FundZakupDataZakupu, [UzytkownikID] = @UzytkownikID, [FundZakupDataDodaniaZakupu] = @FundZakupDataDodaniaZakupu, [FundOplataTypID] = @FundOplataTypID, [FundOplataWysokosc] = @FundOplataWysokosc
                      WHERE ([FundZakupID] = @FundZakupID)"
            );
            
            AlterStoredProcedure(
                "dbo.FundZakup_Delete",
                p => new
                    {
                        FundZakupID = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[FundZakup]
                      WHERE ([FundZakupID] = @FundZakupID)"
            );
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.FundZakup", new[] { "FundOplataTypID" });
            AlterColumn("dbo.FundZakup", "FundOplataTypID", c => c.Int());
            RenameColumn(table: "dbo.FundZakup", name: "FundOplataTypID", newName: "FundOplataTyp_FundOplataTypID");
            CreateIndex("dbo.FundZakup", "FundOplataTyp_FundOplataTypID");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
