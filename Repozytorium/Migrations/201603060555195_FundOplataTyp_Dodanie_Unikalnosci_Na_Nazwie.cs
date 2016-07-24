namespace Repozytorium.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FundOplataTyp_Dodanie_Unikalnosci_Na_Nazwie : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.FundOplataTyp", "FundOplataTypNazwa", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.FundOplataTyp", new[] { "FundOplataTypNazwa" });
        }
    }
}
