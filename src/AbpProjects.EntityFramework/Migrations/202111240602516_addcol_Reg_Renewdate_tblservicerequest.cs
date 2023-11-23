namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcol_Reg_Renewdate_tblservicerequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TblServiceRequest", "RegistrationDate", c => c.DateTime());
            AddColumn("dbo.TblServiceRequest", "NextRenewalDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TblServiceRequest", "NextRenewalDate");
            DropColumn("dbo.TblServiceRequest", "RegistrationDate");
        }
    }
}
