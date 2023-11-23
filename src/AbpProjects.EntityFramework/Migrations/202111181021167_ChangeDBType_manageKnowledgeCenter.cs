namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDBType_manageKnowledgeCenter : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ManageKnowledgeCenter", "IsDocument", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ManageKnowledgeCenter", "IsDocument", c => c.Boolean(nullable: false));
        }
    }
}
