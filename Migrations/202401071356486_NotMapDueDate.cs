namespace Helpdesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotMapDueDate : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Incidents", "DueDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Incidents", "DueDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
    }
}
