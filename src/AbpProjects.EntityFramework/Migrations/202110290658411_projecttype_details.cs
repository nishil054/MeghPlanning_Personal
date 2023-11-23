namespace AbpProjects.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class projecttype_details : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Projecttype_details",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        hours = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TypeID = c.Int(nullable: false),
                        ProjectId = c.Int(nullable: false),
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
                    { "DynamicFilter_Projecttype_details_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Project", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.ProjectType", t => t.TypeID, cascadeDelete: true)
                .Index(t => t.TypeID)
                .Index(t => t.ProjectId)
                .Index(t => t.IsDeleted);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projecttype_details", "TypeID", "dbo.ProjectType");
            DropForeignKey("dbo.Projecttype_details", "ProjectId", "dbo.Project");
            DropIndex("dbo.Projecttype_details", new[] { "IsDeleted" });
            DropIndex("dbo.Projecttype_details", new[] { "ProjectId" });
            DropIndex("dbo.Projecttype_details", new[] { "TypeID" });
            DropTable("dbo.Projecttype_details",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Projecttype_details_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
