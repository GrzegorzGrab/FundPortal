namespace Repozytorium.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FundNameRequied : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Fund", "FundName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Fund", "FundName", c => c.String());
        }
    }
}
