namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsEnableProjectColumnAddInProjectTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Project", "IsEnable", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Project", "IsEnable");
        }
    }
}
