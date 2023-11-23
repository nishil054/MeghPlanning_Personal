namespace AbpProjects.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Added_tbl_User_Story : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbl_User_Story",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserStory = c.String(),
                        DeveloperHours = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpectedHours = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ActualHours = c.Decimal(nullable: false, precision: 18, scale: 2),
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
                    { "DynamicFilter_ImportUserStoryDetails_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.IsDeleted);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.tbl_User_Story", new[] { "IsDeleted" });
            DropTable("dbo.tbl_User_Story",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ImportUserStoryDetails_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
