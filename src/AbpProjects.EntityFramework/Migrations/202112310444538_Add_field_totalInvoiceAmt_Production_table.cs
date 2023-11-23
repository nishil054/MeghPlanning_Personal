namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_field_totalInvoiceAmt_Production_table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Productions", "TotalInvoiceAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Productions", "TotalInvoiceAmount");
        }
    }
}
