namespace AbpProjects.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class InterestedOpportunity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InterestedOpportunities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Opportunityid = c.Int(nullable: false),
                        projectypeid = c.Int(nullable: false),
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
                    { "DynamicFilter_InterestedOpportunity_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.IsDeleted);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.InterestedOpportunities", new[] { "IsDeleted" });
            DropTable("dbo.InterestedOpportunities",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_InterestedOpportunity_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
