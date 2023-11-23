namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class companyid_added_in_companytable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Company", "CompanyId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Company", "CompanyId");
        }
    }
}
