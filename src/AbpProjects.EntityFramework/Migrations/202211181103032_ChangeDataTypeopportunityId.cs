namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDataTypeopportunityId : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OpportunityFollowUps", "opporutnityid", c => c.Int(nullable: false));
            AlterColumn("dbo.OpportunityFollowUps", "nextactiondate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OpportunityFollowUps", "nextactiondate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.OpportunityFollowUps", "opporutnityid", c => c.String());
        }
    }
}
