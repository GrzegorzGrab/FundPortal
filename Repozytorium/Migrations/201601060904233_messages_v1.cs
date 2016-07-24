namespace Repozytorium.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class messages_v1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FundMessages", "Message", c => c.String());
            AddColumn("dbo.FundMessages", "IsRead", c => c.Boolean(nullable: false));
            AlterStoredProcedure(
                "dbo.FundMessages_Insert",
                p => new
                    {
                        UsernName = p.String(),
                        UserEmail = p.String(),
                        Message = p.String(),
                        SentDate = p.DateTime(),
                        IsRead = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[FundMessages]([UsernName], [UserEmail], [Message], [SentDate], [IsRead])
                      VALUES (@UsernName, @UserEmail, @Message, GETDATE(), 0)
                      
                      DECLARE @FundMessagesID int
                      SELECT @FundMessagesID = [FundMessagesID]
                      FROM [dbo].[FundMessages]
                      WHERE @@ROWCOUNT > 0 AND [FundMessagesID] = scope_identity()
                      
                      SELECT t0.[FundMessagesID]
                      FROM [dbo].[FundMessages] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[FundMessagesID] = @FundMessagesID"
            );
            
            AlterStoredProcedure(
                "dbo.FundMessages_Update",
                p => new
                    {
                        FundMessagesID = p.Int(),
                        UsernName = p.String(),
                        UserEmail = p.String(),
                        Message = p.String(),
                        SentDate = p.DateTime(),
                        IsRead = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[FundMessages]
                      SET [UsernName] = @UsernName, [UserEmail] = @UserEmail, [Message] = @Message, [SentDate] = @SentDate, [IsRead] = @IsRead
                      WHERE ([FundMessagesID] = @FundMessagesID)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.FundMessages", "IsRead");
            DropColumn("dbo.FundMessages", "Message");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
