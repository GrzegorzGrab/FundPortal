namespace Repozytorium.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FundZakup_Dodanie_Nowych_Atrybutow : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FundZakup", "FundOplataWysokosc", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterStoredProcedure(
                "dbo.FundZakup_Insert",
                p => new
                    {
                        FundZakupIlosc = p.Decimal(precision: 18, scale: 2),
                        FundZakupCena = p.Decimal(precision: 18, scale: 2),
                        FundID = p.Int(),
                        FundZakupDataZakupu = p.DateTime(),
                        UzytkownikID = p.String(maxLength: 128),
                        FundOplataWysokosc = p.Decimal(precision: 18, scale: 2),
                        FundOplataTyp_FundOplataTypID = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[FundZakup]([FundZakupIlosc], [FundZakupCena], [FundID], [FundZakupDataZakupu], [UzytkownikID], [FundOplataWysokosc], [FundOplataTyp_FundOplataTypID])
                      VALUES (@FundZakupIlosc, @FundZakupCena, @FundID, @FundZakupDataZakupu, @UzytkownikID, @FundOplataWysokosc, @FundOplataTyp_FundOplataTypID)
                      
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
                        FundOplataWysokosc = p.Decimal(precision: 18, scale: 2),
                        FundOplataTyp_FundOplataTypID = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[FundZakup]
                      SET [FundZakupIlosc] = @FundZakupIlosc, [FundZakupCena] = @FundZakupCena, [FundID] = @FundID, [FundZakupDataZakupu] = @FundZakupDataZakupu, [UzytkownikID] = @UzytkownikID, [FundOplataWysokosc] = @FundOplataWysokosc, [FundOplataTyp_FundOplataTypID] = @FundOplataTyp_FundOplataTypID
                      WHERE ([FundZakupID] = @FundZakupID)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.FundZakup", "FundOplataWysokosc");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
