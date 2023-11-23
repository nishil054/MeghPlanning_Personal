namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class increase_width : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TimeSheet", "Description", c => c.String(nullable: false, maxLength: 1000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TimeSheet", "Description", c => c.String(nullable: false, maxLength: 250));
        }
    }
}
