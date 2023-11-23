namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_PerformaInvoicetable_type : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PerformaInvoice", "LastModificationTime", c => c.DateTime());
            AddColumn("dbo.PerformaInvoice", "LastModifierUserId", c => c.Long());
            AddColumn("dbo.PerformaInvoice", "CreationTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.PerformaInvoice", "CreatorUserId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PerformaInvoice", "CreatorUserId");
            DropColumn("dbo.PerformaInvoice", "CreationTime");
            DropColumn("dbo.PerformaInvoice", "LastModifierUserId");
            DropColumn("dbo.PerformaInvoice", "LastModificationTime");
        }
    }
}
