namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_userid_col : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TimeSheet", "UserId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TimeSheet", "UserId");
        }
    }
}
