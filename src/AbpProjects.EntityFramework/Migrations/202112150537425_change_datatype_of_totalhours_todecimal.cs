namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change_datatype_of_totalhours_todecimal : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Project", "totalhours", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Project", "totalhours", c => c.String());
        }
    }
}
