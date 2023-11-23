namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_col_InvoiceNote_in_ServiceRequestHistory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TblServiceRequest", "InvoiceNote", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TblServiceRequest", "InvoiceNote");
        }
    }
}
