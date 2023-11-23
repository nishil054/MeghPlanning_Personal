namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_new_column_exchangerate_bill : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bill", "exchangerate", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bill", "exchangerate");
        }
    }
}
