namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_timesheet_addCol_date : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TimeSheet", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TimeSheet", "Date");
        }
    }
}
