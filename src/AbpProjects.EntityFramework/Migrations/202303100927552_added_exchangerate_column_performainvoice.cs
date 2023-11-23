namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_exchangerate_column_performainvoice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PerformaInvoice", "exchangerate", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PerformaInvoice", "exchangerate");
        }
    }
}
