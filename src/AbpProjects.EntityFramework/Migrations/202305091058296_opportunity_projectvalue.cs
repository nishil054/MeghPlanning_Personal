namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class opportunity_projectvalue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Opportunities", "ProjectValue", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Opportunities", "ProjectValue");
        }
    }
}
