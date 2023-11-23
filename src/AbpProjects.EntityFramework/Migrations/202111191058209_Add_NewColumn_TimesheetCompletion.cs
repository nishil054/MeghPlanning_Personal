namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_NewColumn_TimesheetCompletion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_UserLogIn_LogOut", "TimesheetComplete", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_UserLogIn_LogOut", "TimesheetComplete");
        }
    }
}
