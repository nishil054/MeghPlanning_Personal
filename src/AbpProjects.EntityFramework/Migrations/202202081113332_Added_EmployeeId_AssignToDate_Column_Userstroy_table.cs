namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_EmployeeId_AssignToDate_Column_Userstroy_table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_User_Story", "EmployeeId", c => c.Int(nullable: false));
            AddColumn("dbo.tbl_User_Story", "AssignToDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_User_Story", "AssignToDate");
            DropColumn("dbo.tbl_User_Story", "EmployeeId");
        }
    }
}
