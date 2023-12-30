namespace Helpdesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteIsUnread : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Notifications", "IsUnread");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notifications", "IsUnread", c => c.Boolean(nullable: false));
        }
    }
}
