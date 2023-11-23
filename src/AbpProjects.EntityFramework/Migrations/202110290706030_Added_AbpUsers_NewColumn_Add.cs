namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_AbpUsers_NewColumn_Add : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AbpUsers", "Resigndate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AbpUsers", "Lastdate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AbpUsers", "Lastdate");
            DropColumn("dbo.AbpUsers", "Resigndate");
        }
    }
}
