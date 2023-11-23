namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addnewcolumnHoursInProjectTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Project", "totalhours", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Project", "totalhours");
        }
    }
}
