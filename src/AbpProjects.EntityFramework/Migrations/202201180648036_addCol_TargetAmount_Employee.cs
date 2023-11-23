namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCol_TargetAmount_Employee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AbpUsers", "TargetAmount", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AbpUsers", "TargetAmount");
        }
    }
}
