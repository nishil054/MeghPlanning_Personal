namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveNullValidationActualDateInProject : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Project", "ActualEndDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Project", "ActualEndDate", c => c.DateTime(nullable: false));
        }
    }
}
