namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_BillpaymentRecipttable_type : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BillPymtRecd", "LastModificationTime", c => c.DateTime());
            AddColumn("dbo.BillPymtRecd", "LastModifierUserId", c => c.Long());
            AddColumn("dbo.BillPymtRecd", "CreationTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.BillPymtRecd", "CreatorUserId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BillPymtRecd", "CreatorUserId");
            DropColumn("dbo.BillPymtRecd", "CreationTime");
            DropColumn("dbo.BillPymtRecd", "LastModifierUserId");
            DropColumn("dbo.BillPymtRecd", "LastModificationTime");
        }
    }
}
