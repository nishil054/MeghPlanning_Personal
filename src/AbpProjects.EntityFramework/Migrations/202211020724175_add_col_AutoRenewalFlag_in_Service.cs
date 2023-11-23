namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_col_AutoRenewalFlag_in_Service : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TblManageService", "IsAutoRenewal", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TblManageService", "IsAutoRenewal");
        }
    }
}
