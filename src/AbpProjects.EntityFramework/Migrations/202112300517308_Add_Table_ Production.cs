namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Table_Production : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Productions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Invoiceid = c.Int(nullable: false),
                        Invoicetype = c.Int(nullable: false),
                        Invoicedate = c.DateTime(nullable: false),
                        InvoiceAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Serviceid = c.Int(nullable: false),
                        Projectid = c.Int(nullable: false),
                        Requestid = c.Int(nullable: false),
                        ProductionFlag = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Productions");
        }
    }
}
