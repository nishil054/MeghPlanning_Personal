namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class registrationdate_column_added : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TblManageService", "RegistrationDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TblManageService", "RegistrationDate");
        }
    }
}
