namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnInOpportunityOpportOwner : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Opportunities", "OpportunityOwner", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Opportunities", "OpportunityOwner");
        }
    }
}
