namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDataTypeOfIsDocument_manageKnowledgeCenter : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ManageKnowledgeCenter", "IsDocument", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ManageKnowledgeCenter", "IsDocument", c => c.String());
        }
    }
}
