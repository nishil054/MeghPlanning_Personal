namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_table_Bill : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bill",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BillNo = c.String(nullable: false, maxLength: 7),
                        ClientID = c.Int(),
                        Cancel = c.Boolean(nullable: false),
                        BillDate = c.DateTime(),
                        PrepDate = c.DateTime(),
                        PrepBy = c.String(maxLength: 200),
                        PartnerID = c.String(maxLength: 200),
                        Comment = c.String(maxLength: 50),
                        SerTextID1 = c.Int(),
                        SText1 = c.String(maxLength: 1000),
                        SerTextID2 = c.Int(),
                        SText2 = c.String(maxLength: 1000),
                        SerTextID3 = c.Int(),
                        SText3 = c.String(maxLength: 1000),
                        SerTextID4 = c.Int(),
                        SText4 = c.String(maxLength: 1000),
                        SAmt1 = c.Int(),
                        SAmt2 = c.Int(),
                        SAmt3 = c.Int(),
                        SAmt4 = c.Int(),
                        STax = c.Int(),
                        SCess = c.Int(),
                        SHCess = c.Int(),
                        BillTotal = c.String(maxLength: 100),
                        Currency = c.String(maxLength: 10),
                        servicegroupid = c.Int(),
                        clientgroupid = c.Int(),
                        servicetypeid = c.Int(),
                        totalbillamount = c.Decimal(precision: 18, scale: 2),
                        bankid = c.Int(),
                        servicetypeid2 = c.Int(),
                        servicetypeid3 = c.Int(),
                        servicetypeid4 = c.Int(),
                        swachhtax = c.Decimal(precision: 18, scale: 2),
                        servicetax = c.Decimal(precision: 18, scale: 2),
                        service_year = c.Int(),
                        servicetotal = c.Decimal(precision: 18, scale: 2),
                        address = c.String(maxLength: 200),
                        citypin = c.String(maxLength: 100),
                        isdefault = c.Boolean(),
                        panno = c.String(maxLength: 100),
                        krishitax = c.Decimal(precision: 18, scale: 2),
                        cgst = c.Decimal(precision: 18, scale: 2),
                        sgst = c.Decimal(precision: 18, scale: 2),
                        igst = c.Decimal(precision: 18, scale: 2),
                        userid = c.String(maxLength: 200),
                        statuscodeid = c.Int(),
                        statuscodeno = c.String(maxLength: 100),
                        gstin = c.String(maxLength: 100),
                        sacno1 = c.String(maxLength: 200),
                        sacno2 = c.String(maxLength: 200),
                        sacno3 = c.String(maxLength: 200),
                        sacno4 = c.String(maxLength: 200),
                        isexport = c.Boolean(),
                        companyid = c.Int(),
                        invoiceno = c.String(maxLength: 200),
                        orderno = c.String(maxLength: 100),
                        orderdate = c.DateTime(),
                        isoutexport = c.Boolean(),
                        isoutexport1 = c.Boolean(nullable: false),
                        Performaid = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Bill");
        }
    }
}
