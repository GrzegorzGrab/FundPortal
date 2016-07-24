namespace Repozytorium.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uzupelnienie_atrybutow_waluty : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.FundWaluta", new[] { "FundWalutaKod" });
            AlterColumn("dbo.FundWaluta", "FundWalutaKod", c => c.String(nullable: false, maxLength: 3));
            AlterColumn("dbo.FundWaluta", "FundWalutaPelnaNazwa", c => c.String(nullable: false));
            CreateIndex("dbo.FundWaluta", "FundWalutaKod", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.FundWaluta", new[] { "FundWalutaKod" });
            AlterColumn("dbo.FundWaluta", "FundWalutaPelnaNazwa", c => c.String());
            AlterColumn("dbo.FundWaluta", "FundWalutaKod", c => c.String(maxLength: 3));
            CreateIndex("dbo.FundWaluta", "FundWalutaKod", unique: true);
        }
    }
}
