namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_col_Term_in_Service : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TblManageService", "Term", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TblManageService", "Term");
        }
    }
}
