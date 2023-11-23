namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_NewColumn_ProjectId_tbl_User_Story : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_User_Story", "ProjectId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_User_Story", "ProjectId");
        }
    }
}
