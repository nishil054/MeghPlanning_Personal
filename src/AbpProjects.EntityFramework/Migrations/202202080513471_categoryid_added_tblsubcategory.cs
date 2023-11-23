namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class categoryid_added_tblsubcategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExpenseSubcategories", "CategoryId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ExpenseSubcategories", "CategoryId");
        }
    }
}
