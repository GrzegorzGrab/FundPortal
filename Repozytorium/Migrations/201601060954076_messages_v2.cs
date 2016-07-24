namespace Repozytorium.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class messages_v2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FundMessages", "UsernName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FundMessages", "UsernName", c => c.String());
        }
    }
}
