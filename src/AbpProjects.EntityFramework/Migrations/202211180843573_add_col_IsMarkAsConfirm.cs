namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_col_IsMarkAsConfirm : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PerformaInvoice", "IsMarkAsConfirm", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PerformaInvoice", "IsMarkAsConfirm");
        }
    }
}
