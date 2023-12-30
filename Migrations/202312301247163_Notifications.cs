namespace Helpdesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Notifications : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        NotificationId = c.Int(nullable: false, identity: true),
                        NotificationSubject = c.String(),
                        SenderId = c.String(maxLength: 128),
                        RecipientId = c.String(maxLength: 128),
                        NotificationDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        IsRead = c.Boolean(nullable: false),
                        IsUnread = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.NotificationId)
                .ForeignKey("dbo.AspNetUsers", t => t.RecipientId)
                .ForeignKey("dbo.AspNetUsers", t => t.SenderId)
                .Index(t => t.SenderId)
                .Index(t => t.RecipientId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "SenderId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Notifications", "RecipientId", "dbo.AspNetUsers");
            DropIndex("dbo.Notifications", new[] { "RecipientId" });
            DropIndex("dbo.Notifications", new[] { "SenderId" });
            DropTable("dbo.Notifications");
        }
    }
}
