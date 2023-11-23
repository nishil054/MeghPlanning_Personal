namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class remove_requirefld_EnddtTeamEnd_Project : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Project", "EndDate", c => c.DateTime());
            AlterColumn("dbo.Project", "TeamDeadline", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Project", "TeamDeadline", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Project", "EndDate", c => c.DateTime(nullable: false));
        }
    }
}
