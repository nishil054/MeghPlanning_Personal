namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumn_Category_ManageKnowledgeCenter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ManageKnowledgeCenter", "CategoryId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ManageKnowledgeCenter", "CategoryId");
        }
    }
}
