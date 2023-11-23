namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class col_added_databasespace_in_manageservice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TblManageService", "DatabaseSpace", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TblManageService", "DatabaseSpace");
        }
    }
}
