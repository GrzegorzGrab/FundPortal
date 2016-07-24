namespace Repozytorium.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pole_wymagane_dla_typu_funduszu : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FundTyp", "FundTypName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FundTyp", "FundTypName", c => c.String());
        }
    }
}
