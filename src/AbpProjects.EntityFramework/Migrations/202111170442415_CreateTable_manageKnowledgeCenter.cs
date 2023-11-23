namespace AbpProjects.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTable_manageKnowledgeCenter : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ManageKnowledgeCenter",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TeamId = c.Int(nullable: false),
                        Title = c.String(),
                        IsDocument = c.Boolean(nullable: false),
                        Url = c.String(),
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
                    { "DynamicFilter_manageKnowledgeCenter_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.IsDeleted);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ManageKnowledgeCenter", new[] { "IsDeleted" });
            DropTable("dbo.ManageKnowledgeCenter",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_manageKnowledgeCenter_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
