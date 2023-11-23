namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class priority_field_added_in_projecttbl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Project", "Priority", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Project", "Priority");
        }
    }
}
