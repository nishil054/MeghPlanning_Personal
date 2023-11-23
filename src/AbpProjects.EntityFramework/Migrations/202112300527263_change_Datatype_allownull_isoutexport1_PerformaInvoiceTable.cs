namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change_Datatype_allownull_isoutexport1_PerformaInvoiceTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PerformaInvoice", "isoutexport1", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PerformaInvoice", "isoutexport1", c => c.Boolean(nullable: false));
        }
    }
}
