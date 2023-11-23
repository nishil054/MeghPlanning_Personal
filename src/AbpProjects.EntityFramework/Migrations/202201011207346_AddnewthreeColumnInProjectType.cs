namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddnewthreeColumnInProjectType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projecttype_details", "IsOutSource", c => c.Boolean());
            AddColumn("dbo.Projecttype_details", "CostforCompany", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Projecttype_details", "Comments", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projecttype_details", "Comments");
            DropColumn("dbo.Projecttype_details", "CostforCompany");
            DropColumn("dbo.Projecttype_details", "IsOutSource");
        }
    }
}
