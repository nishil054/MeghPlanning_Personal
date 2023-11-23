namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_ColumnSet_Nullabe : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tbl_UserLogIn_LogOut", "LoggedOut", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbl_UserLogIn_LogOut", "LoggedOut", c => c.DateTime(nullable: false));
        }
    }
}
