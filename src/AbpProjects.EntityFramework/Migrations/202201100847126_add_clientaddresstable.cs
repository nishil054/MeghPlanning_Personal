namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_clientaddresstable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbl_ClientAddress",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        clientid = c.Int(),
                        clientaddress = c.String(maxLength: 200),
                        city = c.String(maxLength: 50),
                        state = c.String(maxLength: 50),
                        pincode = c.String(maxLength: 7),
                        Contactname = c.String(maxLength: 100),
                        Email = c.String(maxLength: 50),
                        Contactno = c.String(maxLength: 10),
                        isdefault = c.Boolean(),
                        statecodeid = c.Int(),
                        gstno = c.String(maxLength: 100),
                        CountryName = c.String(maxLength: 200),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tbl_ClientAddress");
        }
    }
}
