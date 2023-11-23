namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_New_Column_UserStoryId_Timesheet : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TimeSheet", "UserStoryId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TimeSheet", "UserStoryId");
        }
    }
}
