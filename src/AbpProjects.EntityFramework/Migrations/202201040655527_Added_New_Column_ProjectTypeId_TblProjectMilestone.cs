namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_New_Column_ProjectTypeId_TblProjectMilestone : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TblProjectMilestone", "ProjectTypeId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TblProjectMilestone", "ProjectTypeId");
        }
    }
}
