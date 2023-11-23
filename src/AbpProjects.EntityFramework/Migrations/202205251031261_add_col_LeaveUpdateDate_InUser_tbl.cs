namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_col_LeaveUpdateDate_InUser_tbl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AbpUsers", "LeaveUpdateDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AbpUsers", "LeaveUpdateDate");
        }
    }
}
