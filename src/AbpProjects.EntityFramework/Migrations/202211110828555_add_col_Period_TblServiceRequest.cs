namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_col_Period_TblServiceRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TblServiceRequest", "Period", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TblServiceRequest", "Period");
        }
    }
}
