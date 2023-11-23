namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OpportunityidAddInProjectTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Project", "Opportunityid", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Project", "Opportunityid");
        }
    }
}
