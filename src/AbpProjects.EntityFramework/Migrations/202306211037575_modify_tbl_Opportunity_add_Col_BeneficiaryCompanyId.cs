namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_tbl_Opportunity_add_Col_BeneficiaryCompanyId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Opportunities", "BeneficiaryCompanyId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Opportunities", "BeneficiaryCompanyId");
        }
    }
}
