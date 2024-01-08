namespace Helpdesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDueDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Incidents", "DueDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Incidents", "DueDate");
        }
    }
}
