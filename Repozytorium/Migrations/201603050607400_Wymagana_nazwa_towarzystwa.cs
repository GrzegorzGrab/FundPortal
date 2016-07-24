namespace Repozytorium.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Wymagana_nazwa_towarzystwa : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FundTowarzystwo", "FundTowarzystwoNazwa", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FundTowarzystwo", "FundTowarzystwoNazwa", c => c.String());
        }
    }
}
