namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnTable_manageKnowledgeCenter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ManageKnowledgeCenter", "Comment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ManageKnowledgeCenter", "Comment");
        }
    }
}
