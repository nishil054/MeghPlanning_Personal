namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveValidationInCompanyNameInProjectTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Project", "CompanyName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Project", "CompanyName", c => c.String(nullable: false, maxLength: 250));
        }
    }
}
