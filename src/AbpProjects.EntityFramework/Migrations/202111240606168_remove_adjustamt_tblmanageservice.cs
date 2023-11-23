namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class remove_adjustamt_tblmanageservice : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TblManageService", "AdjustmentAmount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TblManageService", "AdjustmentAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
