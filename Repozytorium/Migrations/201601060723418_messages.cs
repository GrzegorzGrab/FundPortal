namespace Repozytorium.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class messages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FundMessages",
                c => new
                    {
                        FundMessagesID = c.Int(nullable: false, identity: true),
                        UsernName = c.String(),
                        UserEmail = c.String(),
                        SentDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.FundMessagesID);
            
            CreateStoredProcedure(
                "dbo.FundMessages_Insert",
                p => new
                    {
                        UsernName = p.String(),
                        UserEmail = p.String(),
                        SentDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[FundMessages]([UsernName], [UserEmail], [SentDate])
                      VALUES (@UsernName, @UserEmail, @SentDate)
                      
                      DECLARE @FundMessagesID int
                      SELECT @FundMessagesID = [FundMessagesID]
                      FROM [dbo].[FundMessages]
                      WHERE @@ROWCOUNT > 0 AND [FundMessagesID] = scope_identity()
                      
                      SELECT t0.[FundMessagesID]
                      FROM [dbo].[FundMessages] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[FundMessagesID] = @FundMessagesID"
            );
            
            CreateStoredProcedure(
                "dbo.FundMessages_Update",
                p => new
                    {
                        FundMessagesID = p.Int(),
                        UsernName = p.String(),
                        UserEmail = p.String(),
                        SentDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[FundMessages]
                      SET [UsernName] = @UsernName, [UserEmail] = @UserEmail, [SentDate] = @SentDate
                      WHERE ([FundMessagesID] = @FundMessagesID)"
            );
            
            CreateStoredProcedure(
                "dbo.FundMessages_Delete",
                p => new
                    {
                        FundMessagesID = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[FundMessages]
                      WHERE ([FundMessagesID] = @FundMessagesID)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.FundMessages_Delete");
            DropStoredProcedure("dbo.FundMessages_Update");
            DropStoredProcedure("dbo.FundMessages_Insert");
            DropTable("dbo.FundMessages");
        }
    }
}
