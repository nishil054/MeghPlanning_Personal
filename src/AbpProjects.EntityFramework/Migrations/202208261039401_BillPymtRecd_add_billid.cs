namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BillPymtRecd_add_billid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BillPymtRecd", "Billid", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BillPymtRecd", "Billid");
        }
    }
}
