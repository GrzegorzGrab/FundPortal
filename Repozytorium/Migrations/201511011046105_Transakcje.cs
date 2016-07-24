namespace Repozytorium.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Transakcje : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Transakcja",
                c => new
                    {
                        TransakcjaID = c.Int(nullable: false, identity: true),
                        FundID = c.Int(nullable: false),
                        UzytkownikId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.TransakcjaID)
                .ForeignKey("dbo.Fund", t => t.FundID)
                .ForeignKey("dbo.AspNetUsers", t => t.UzytkownikId)
                .Index(t => t.FundID)
                .Index(t => t.UzytkownikId);
            
            AddColumn("dbo.AspNetUsers", "Login", c => c.String());
            //DropColumn("dbo.AspNetUsers", "UzytkownikID");
            CreateStoredProcedure(
                "dbo.Transakcja_Insert",
                p => new
                    {
                        FundID = p.Int(),
                        UzytkownikId = p.String(maxLength: 128),
                    },
                body:
                    @"INSERT [dbo].[Transakcja]([FundID], [UzytkownikId])
                      VALUES (@FundID, @UzytkownikId)
                      
                      DECLARE @TransakcjaID int
                      SELECT @TransakcjaID = [TransakcjaID]
                      FROM [dbo].[Transakcja]
                      WHERE @@ROWCOUNT > 0 AND [TransakcjaID] = scope_identity()
                      
                      SELECT t0.[TransakcjaID]
                      FROM [dbo].[Transakcja] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[TransakcjaID] = @TransakcjaID"
            );
            
            CreateStoredProcedure(
                "dbo.Transakcja_Update",
                p => new
                    {
                        TransakcjaID = p.Int(),
                        FundID = p.Int(),
                        UzytkownikId = p.String(maxLength: 128),
                    },
                body:
                    @"UPDATE [dbo].[Transakcja]
                      SET [FundID] = @FundID, [UzytkownikId] = @UzytkownikId
                      WHERE ([TransakcjaID] = @TransakcjaID)"
            );
            
            CreateStoredProcedure(
                "dbo.Transakcja_Delete",
                p => new
                    {
                        TransakcjaID = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Transakcja]
                      WHERE ([TransakcjaID] = @TransakcjaID)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.Transakcja_Delete");
            DropStoredProcedure("dbo.Transakcja_Update");
            DropStoredProcedure("dbo.Transakcja_Insert");
            AddColumn("dbo.AspNetUsers", "UzytkownikID", c => c.Int());
            DropForeignKey("dbo.Transakcja", "UzytkownikId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Transakcja", "FundID", "dbo.Fund");
            DropIndex("dbo.Transakcja", new[] { "UzytkownikId" });
            DropIndex("dbo.Transakcja", new[] { "FundID" });
            DropColumn("dbo.AspNetUsers", "Login");
            DropTable("dbo.Transakcja");
        }
    }
}
