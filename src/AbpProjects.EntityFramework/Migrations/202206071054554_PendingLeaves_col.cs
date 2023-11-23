namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PendingLeaves_col : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AbpUsers", "PendingLeaves", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AbpUsers", "PendingLeaves");
        }
    }
}
