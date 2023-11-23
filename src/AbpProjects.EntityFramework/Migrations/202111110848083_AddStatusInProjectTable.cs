namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStatusInProjectTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Project", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Project", "Status");
        }
    }
}
