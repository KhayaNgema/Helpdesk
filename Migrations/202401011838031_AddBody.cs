namespace Helpdesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBody : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "FullSenderName", c => c.String());
            AddColumn("dbo.Notifications", "NotificationBody", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notifications", "NotificationBody");
            DropColumn("dbo.Notifications", "FullSenderName");
        }
    }
}
