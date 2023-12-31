namespace Helpdesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsNew : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "IsNew", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notifications", "IsNew");
        }
    }
}
