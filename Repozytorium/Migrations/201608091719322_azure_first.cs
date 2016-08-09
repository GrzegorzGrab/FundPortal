namespace Repozytorium.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class azure_first : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.FundZakup", new[] { "FundOplataTyp_FundOplataTypID" });
            RenameColumn(table: "dbo.FundZakup", name: "FundOplataTyp_FundOplataTypID", newName: "FundOplataTypID");
            CreateTable(
                "dbo.FundFile",
                c => new
                    {
                        FundFileID = c.Int(nullable: false, identity: true),
                        FundFileName = c.String(),
                        FundType = c.String(),
                        FundContent = c.Binary(),
                    })
                .PrimaryKey(t => t.FundFileID);
            
            AddColumn("dbo.FundZakup", "FundZakupDataDodaniaZakupu", c => c.DateTime(nullable: false));
            AddColumn("dbo.FundZakup", "FundOplataWysokosc", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Fund", "FundSymbol", c => c.String(nullable: false));
            AddColumn("dbo.Fund", "FundZarzadzajacy", c => c.String());
            AddColumn("dbo.Fund", "FundDataUruchomienia", c => c.DateTime(nullable: false));
            AddColumn("dbo.Fund", "FundPierwszajWplata", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Fund", "FundNastepnaWplata", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Fund", "FundPolityka", c => c.String());
            AddColumn("dbo.Fund", "FundFileID", c => c.Int());
            AddColumn("dbo.FundTowarzystwo", "FundTowarzystwoAdresUrl", c => c.String());
            AddColumn("dbo.FundWycena", "FundWycenaCenaMin", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.FundWycena", "FundWycenaCenaMax", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.FundZakup", "FundOplataTypID", c => c.Int(nullable: false));
            CreateIndex("dbo.FundZakup", "FundOplataTypID");
            CreateIndex("dbo.Fund", "FundFileID");
            AddForeignKey("dbo.Fund", "FundFileID", "dbo.FundFile", "FundFileID");
            CreateStoredProcedure(
                "dbo.FundFile_Insert",
                p => new
                    {
                        FundFileName = p.String(),
                        FundType = p.String(),
                        FundContent = p.Binary(),
                    },
                body:
                    @"INSERT [dbo].[FundFile]([FundFileName], [FundType], [FundContent])
                      VALUES (@FundFileName, @FundType, @FundContent)
                      
                      DECLARE @FundFileID int
                      SELECT @FundFileID = [FundFileID]
                      FROM [dbo].[FundFile]
                      WHERE @@ROWCOUNT > 0 AND [FundFileID] = scope_identity()
                      
                      SELECT t0.[FundFileID]
                      FROM [dbo].[FundFile] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[FundFileID] = @FundFileID"
            );
            
            CreateStoredProcedure(
                "dbo.FundFile_Update",
                p => new
                    {
                        FundFileID = p.Int(),
                        FundFileName = p.String(),
                        FundType = p.String(),
                        FundContent = p.Binary(),
                    },
                body:
                    @"UPDATE [dbo].[FundFile]
                      SET [FundFileName] = @FundFileName, [FundType] = @FundType, [FundContent] = @FundContent
                      WHERE ([FundFileID] = @FundFileID)"
            );
            
            CreateStoredProcedure(
                "dbo.FundFile_Delete",
                p => new
                    {
                        FundFileID = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[FundFile]
                      WHERE ([FundFileID] = @FundFileID)"
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
            
            AlterStoredProcedure(
                "dbo.Fund_Insert",
                p => new
                    {
                        FundName = p.String(),
                        FundTypID = p.Int(),
                        FundTowarzystwoID = p.Int(),
                        FundWalutaID = p.Int(),
                        FundSymbol = p.String(),
                        FundZarzadzajacy = p.String(),
                        FundDataUruchomienia = p.DateTime(),
                        FundPierwszajWplata = p.Decimal(precision: 18, scale: 2),
                        FundNastepnaWplata = p.Decimal(precision: 18, scale: 2),
                        FundPolityka = p.String(),
                        FundFileID = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Fund]([FundName], [FundTypID], [FundTowarzystwoID], [FundWalutaID], [FundSymbol], [FundZarzadzajacy], [FundDataUruchomienia], [FundPierwszajWplata], [FundNastepnaWplata], [FundPolityka], [FundFileID])
                      VALUES (@FundName, @FundTypID, @FundTowarzystwoID, @FundWalutaID, @FundSymbol, @FundZarzadzajacy, @FundDataUruchomienia, @FundPierwszajWplata, @FundNastepnaWplata, @FundPolityka, @FundFileID)
                      
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
                        FundSymbol = p.String(),
                        FundZarzadzajacy = p.String(),
                        FundDataUruchomienia = p.DateTime(),
                        FundPierwszajWplata = p.Decimal(precision: 18, scale: 2),
                        FundNastepnaWplata = p.Decimal(precision: 18, scale: 2),
                        FundPolityka = p.String(),
                        FundFileID = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Fund]
                      SET [FundName] = @FundName, [FundTypID] = @FundTypID, [FundTowarzystwoID] = @FundTowarzystwoID, [FundWalutaID] = @FundWalutaID, [FundSymbol] = @FundSymbol, [FundZarzadzajacy] = @FundZarzadzajacy, [FundDataUruchomienia] = @FundDataUruchomienia, [FundPierwszajWplata] = @FundPierwszajWplata, [FundNastepnaWplata] = @FundNastepnaWplata, [FundPolityka] = @FundPolityka, [FundFileID] = @FundFileID
                      WHERE ([FundID] = @FundID)"
            );
            
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
            
            AlterStoredProcedure(
                "dbo.FundWycena_Insert",
                p => new
                    {
                        FundWycenaDate = p.DateTime(),
                        FundWycenaCena = p.Decimal(precision: 18, scale: 2),
                        FundWycenaCenaMin = p.Decimal(precision: 18, scale: 2),
                        FundWycenaCenaMax = p.Decimal(precision: 18, scale: 2),
                        FundID = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[FundWycena]([FundWycenaDate], [FundWycenaCena], [FundWycenaCenaMin], [FundWycenaCenaMax], [FundID])
                      VALUES (@FundWycenaDate, @FundWycenaCena, @FundWycenaCenaMin, @FundWycenaCenaMax, @FundID)
                      
                      DECLARE @FundWycenaID int
                      SELECT @FundWycenaID = [FundWycenaID]
                      FROM [dbo].[FundWycena]
                      WHERE @@ROWCOUNT > 0 AND [FundWycenaID] = scope_identity()
                      
                      SELECT t0.[FundWycenaID]
                      FROM [dbo].[FundWycena] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[FundWycenaID] = @FundWycenaID"
            );
            
            AlterStoredProcedure(
                "dbo.FundWycena_Update",
                p => new
                    {
                        FundWycenaID = p.Int(),
                        FundWycenaDate = p.DateTime(),
                        FundWycenaCena = p.Decimal(precision: 18, scale: 2),
                        FundWycenaCenaMin = p.Decimal(precision: 18, scale: 2),
                        FundWycenaCenaMax = p.Decimal(precision: 18, scale: 2),
                        FundID = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[FundWycena]
                      SET [FundWycenaDate] = @FundWycenaDate, [FundWycenaCena] = @FundWycenaCena, [FundWycenaCenaMin] = @FundWycenaCenaMin, [FundWycenaCenaMax] = @FundWycenaCenaMax, [FundID] = @FundID
                      WHERE ([FundWycenaID] = @FundWycenaID)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.FundFile_Delete");
            DropStoredProcedure("dbo.FundFile_Update");
            DropStoredProcedure("dbo.FundFile_Insert");
            DropForeignKey("dbo.Fund", "FundFileID", "dbo.FundFile");
            DropIndex("dbo.Fund", new[] { "FundFileID" });
            DropIndex("dbo.FundZakup", new[] { "FundOplataTypID" });
            AlterColumn("dbo.FundZakup", "FundOplataTypID", c => c.Int());
            DropColumn("dbo.FundWycena", "FundWycenaCenaMax");
            DropColumn("dbo.FundWycena", "FundWycenaCenaMin");
            DropColumn("dbo.FundTowarzystwo", "FundTowarzystwoAdresUrl");
            DropColumn("dbo.Fund", "FundFileID");
            DropColumn("dbo.Fund", "FundPolityka");
            DropColumn("dbo.Fund", "FundNastepnaWplata");
            DropColumn("dbo.Fund", "FundPierwszajWplata");
            DropColumn("dbo.Fund", "FundDataUruchomienia");
            DropColumn("dbo.Fund", "FundZarzadzajacy");
            DropColumn("dbo.Fund", "FundSymbol");
            DropColumn("dbo.FundZakup", "FundOplataWysokosc");
            DropColumn("dbo.FundZakup", "FundZakupDataDodaniaZakupu");
            DropTable("dbo.FundFile");
            RenameColumn(table: "dbo.FundZakup", name: "FundOplataTypID", newName: "FundOplataTyp_FundOplataTypID");
            CreateIndex("dbo.FundZakup", "FundOplataTyp_FundOplataTypID");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
