namespace AbpProjects.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Add_OpportunityFollowUp_Tables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FollowupIntrests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        followupid = c.Int(nullable: false),
                        intestedid = c.Int(nullable: false),
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
                    { "DynamicFilter_FollowupIntrest_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.IsDeleted);
            
            CreateTable(
                "dbo.OpportunityFollowUps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        opporutnityid = c.String(),
                        nextactiondate = c.DateTime(nullable: false),
                        expectedclosingdate = c.DateTime(),
                        Comment = c.String(),
                        CalllCategoryId = c.Int(nullable: false),
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
                    { "DynamicFilter_OpportunityFollowUp_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.IsDeleted);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.OpportunityFollowUps", new[] { "IsDeleted" });
            DropIndex("dbo.FollowupIntrests", new[] { "IsDeleted" });
            DropTable("dbo.OpportunityFollowUps",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_OpportunityFollowUp_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FollowupIntrests",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FollowupIntrest_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
