namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClientIdInProjectTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Project", "ClientId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Project", "ClientId");
        }
    }
}
