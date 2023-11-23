namespace AbpProjects.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class OpportunityTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Opportunities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(),
                        PersonName = c.String(),
                        EmailId = c.String(),
                        MobileNumber = c.String(),
                        CalllCategoryId = c.Int(nullable: false),
                        AssignUserId = c.Int(nullable: false),
                        Comment = c.String(),
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
                    { "DynamicFilter_Opportunity_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.IsDeleted);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Opportunities", new[] { "IsDeleted" });
            DropTable("dbo.Opportunities",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Opportunity_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
