namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change_Datatype_allownull_isoutexport1_billTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Bill", "isoutexport1", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Bill", "isoutexport1", c => c.Boolean(nullable: false));
        }
    }
}
