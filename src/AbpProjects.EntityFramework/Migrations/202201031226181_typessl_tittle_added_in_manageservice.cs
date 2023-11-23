namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class typessl_tittle_added_in_manageservice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TblManageService", "Typeofssl", c => c.String());
            AddColumn("dbo.TblManageService", "Title", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TblManageService", "Title");
            DropColumn("dbo.TblManageService", "Typeofssl");
        }
    }
}
