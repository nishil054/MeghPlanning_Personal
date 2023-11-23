namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tbl_manageservice_cancelflag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TblManageService", "Cancelflag", c => c.Boolean());
            AddColumn("dbo.TblManageService", "CancelDate", c => c.DateTime());
            AddColumn("dbo.TblManageService", "RenewalDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TblManageService", "RenewalDate");
            DropColumn("dbo.TblManageService", "CancelDate");
            DropColumn("dbo.TblManageService", "Cancelflag");
        }
    }
}
