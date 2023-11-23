namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class projectid_added_tblinvoicerequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InvoiceRequest", "ProjectId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InvoiceRequest", "ProjectId");
        }
    }
}
