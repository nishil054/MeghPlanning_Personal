namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_ColumnNullable_UserLoginData : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tbl_UserLogIn_LogOut", "TimesheetComplete", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbl_UserLogIn_LogOut", "TimesheetComplete", c => c.Int(nullable: false));
        }
    }
}
