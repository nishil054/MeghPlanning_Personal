namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changes_UserStoryId_SetNullableIn_Timesheet : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TimeSheet", "UserStoryId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TimeSheet", "UserStoryId", c => c.Int(nullable: false));
        }
    }
}
