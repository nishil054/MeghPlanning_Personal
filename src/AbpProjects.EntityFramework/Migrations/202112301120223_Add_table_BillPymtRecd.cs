namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_table_BillPymtRecd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BillPymtRecd",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RcptID = c.Int(nullable: false),
                        BillNo = c.String(maxLength: 7),
                        FullPayment = c.Boolean(nullable: false),
                        RcptDate = c.DateTime(),
                        PymtRecd = c.Int(),
                        Mode = c.String(maxLength: 6),
                        ChequeNo = c.String(maxLength: 10),
                        Bank = c.String(maxLength: 30),
                        ChequeDate = c.String(maxLength: 20),
                        TDS = c.Int(),
                        Writeoff = c.Int(),
                        RSTax = c.Int(),
                        RCess = c.Int(),
                        RHCess = c.Int(),
                        bankid = c.Int(),
                        swachhtax = c.Decimal(precision: 18, scale: 2),
                        paymentamount = c.Int(),
                        servicetax = c.Decimal(precision: 18, scale: 2),
                        krishitax = c.Decimal(precision: 18, scale: 2),
                        igst = c.Decimal(precision: 18, scale: 2),
                        sgst = c.Decimal(precision: 18, scale: 2),
                        cgst = c.Decimal(precision: 18, scale: 2),
                        userid = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BillPymtRecd");
        }
    }
}
