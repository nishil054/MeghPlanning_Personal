namespace AbpProjects.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTable_KnowledgeDocuments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.KnowledgeDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        KnowledgeCenterId = c.Int(nullable: false),
                        FileName = c.String(),
                        FilePath = c.String(),
                        DocumentName = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_KnowledgeDocuments_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.IsDeleted);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.KnowledgeDocuments", new[] { "IsDeleted" });
            DropTable("dbo.KnowledgeDocuments",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_KnowledgeDocuments_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
