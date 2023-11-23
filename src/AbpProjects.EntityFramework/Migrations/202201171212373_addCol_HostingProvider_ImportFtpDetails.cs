namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCol_HostingProvider_ImportFtpDetails : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ImportFTPDetails", "HostingProvider", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ImportFTPDetails", "HostingProvider");
        }
    }
}
