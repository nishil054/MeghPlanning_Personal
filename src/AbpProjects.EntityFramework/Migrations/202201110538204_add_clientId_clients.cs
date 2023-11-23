namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_clientId_clients : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "Clientid", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clients", "Clientid");
        }
    }
}
