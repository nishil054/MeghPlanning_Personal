namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changes_tbl_User_Story_ColumnSetNullabe : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tbl_User_Story", "DeveloperHours", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.tbl_User_Story", "ExpectedHours", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.tbl_User_Story", "ActualHours", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbl_User_Story", "ActualHours", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.tbl_User_Story", "ExpectedHours", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.tbl_User_Story", "DeveloperHours", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
