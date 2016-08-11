namespace Repozytorium.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class zmiany_dla_azure : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.FundOplataTyp", new[] { "FundOplataTypNazwa" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.FundOplataTyp", "FundOplataTypNazwa", unique: true);
        }
    }
}
