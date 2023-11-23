namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_col_Period_InvoiceRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InvoiceRequest", "Period", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.InvoiceRequest", "Period");
        }
    }
}
