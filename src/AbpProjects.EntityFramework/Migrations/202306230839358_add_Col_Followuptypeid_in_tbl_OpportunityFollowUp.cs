namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_Col_Followuptypeid_in_tbl_OpportunityFollowUp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OpportunityFollowUps", "Followuptypeid", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OpportunityFollowUps", "Followuptypeid");
        }
    }
}
