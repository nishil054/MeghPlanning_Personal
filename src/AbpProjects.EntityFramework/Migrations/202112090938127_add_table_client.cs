namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_table_client : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClientName = c.String(),
                        GroupID = c.Int(nullable: false),
                        ClientAddr1 = c.String(),
                        ClientAddr2 = c.String(),
                        ClientCity = c.String(),
                        ClientState = c.String(),
                        ClientPIN = c.String(),
                        ClientContact = c.String(),
                        ClientEmail = c.String(),
                        UnderService = c.Boolean(nullable: false),
                        TANNO = c.String(),
                        PANNO = c.String(),
                        sectorid = c.Int(nullable: false),
                        companyid = c.Int(nullable: false),
                        clientcode = c.String(),
                        pan_no = c.String(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Clients");
        }
    }
}
