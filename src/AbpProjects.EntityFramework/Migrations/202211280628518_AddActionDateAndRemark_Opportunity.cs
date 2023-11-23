namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddActionDateAndRemark_Opportunity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Opportunities", "ActionDate", c => c.DateTime());
            AddColumn("dbo.Opportunities", "Remarks", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Opportunities", "Remarks");
            DropColumn("dbo.Opportunities", "ActionDate");
        }
    }
}
