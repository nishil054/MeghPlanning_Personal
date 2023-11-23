namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumn_LeaveBalance_AbpUsers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AbpUsers", "LeaveBalance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AbpUsers", "LeaveBalance");
        }
    }
}
