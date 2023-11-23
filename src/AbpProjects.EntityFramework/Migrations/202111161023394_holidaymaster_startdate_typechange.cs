namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class holidaymaster_startdate_typechange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.HolidayMaster", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.HolidayMaster", "EndDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.HolidayMaster", "EndDate", c => c.String());
            AlterColumn("dbo.HolidayMaster", "StartDate", c => c.String());
        }
    }
}
