namespace Repozytorium.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Zakup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FundZakup",
                c => new
                    {
                        FundZakupID = c.Int(nullable: false, identity: true),
                        FundZakupIlosc = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FundZakupCena = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FundID = c.Int(nullable: false),
                        FundZakupDataZakupu = c.DateTime(nullable: false),
                        UzytkownikID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.FundZakupID)
                .ForeignKey("dbo.Fund", t => t.FundID)
                .ForeignKey("dbo.AspNetUsers", t => t.UzytkownikID)
                .Index(t => t.FundID)
                .Index(t => t.UzytkownikID);
            
            CreateStoredProcedure(
                "dbo.FundZakup_Insert",
                p => new
                    {
                        FundZakupIlosc = p.Decimal(precision: 18, scale: 2),
                        FundZakupCena = p.Decimal(precision: 18, scale: 2),
                        FundID = p.Int(),
                        FundZakupDataZakupu = p.DateTime(),
                        UzytkownikID = p.String(maxLength: 128),
                    },
                body:
                    @"INSERT [dbo].[FundZakup]([FundZakupIlosc], [FundZakupCena], [FundID], [FundZakupDataZakupu], [UzytkownikID])
                      VALUES (@FundZakupIlosc, @FundZakupCena, @FundID, @FundZakupDataZakupu, @UzytkownikID)
                      
                      DECLARE @FundZakupID int
                      SELECT @FundZakupID = [FundZakupID]
                      FROM [dbo].[FundZakup]
                      WHERE @@ROWCOUNT > 0 AND [FundZakupID] = scope_identity()
                      
                      SELECT t0.[FundZakupID]
                      FROM [dbo].[FundZakup] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[FundZakupID] = @FundZakupID"
            );
            
            CreateStoredProcedure(
                "dbo.FundZakup_Update",
                p => new
                    {
                        FundZakupID = p.Int(),
                        FundZakupIlosc = p.Decimal(precision: 18, scale: 2),
                        FundZakupCena = p.Decimal(precision: 18, scale: 2),
                        FundID = p.Int(),
                        FundZakupDataZakupu = p.DateTime(),
                        UzytkownikID = p.String(maxLength: 128),
                    },
                body:
                    @"UPDATE [dbo].[FundZakup]
                      SET [FundZakupIlosc] = @FundZakupIlosc, [FundZakupCena] = @FundZakupCena, [FundID] = @FundID, [FundZakupDataZakupu] = @FundZakupDataZakupu, [UzytkownikID] = @UzytkownikID
                      WHERE ([FundZakupID] = @FundZakupID)"
            );
            
            CreateStoredProcedure(
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
            DropStoredProcedure("dbo.FundZakup_Delete");
            DropStoredProcedure("dbo.FundZakup_Update");
            DropStoredProcedure("dbo.FundZakup_Insert");
            DropForeignKey("dbo.FundZakup", "UzytkownikID", "dbo.AspNetUsers");
            DropForeignKey("dbo.FundZakup", "FundID", "dbo.Fund");
            DropIndex("dbo.FundZakup", new[] { "UzytkownikID" });
            DropIndex("dbo.FundZakup", new[] { "FundID" });
            DropTable("dbo.FundZakup");
        }
    }
}
