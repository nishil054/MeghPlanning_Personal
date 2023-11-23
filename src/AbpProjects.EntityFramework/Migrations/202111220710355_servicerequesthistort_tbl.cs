namespace AbpProjects.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class servicerequesthistort_tbl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TblServiceRequest",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServiceId = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AdjustmentAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Comment = c.String(),
                        Actiontype = c.Int(nullable: false),
                        ActionName = c.String(),
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
                    { "DynamicFilter_ServiceRequestHistory_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.IsDeleted);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.TblServiceRequest", new[] { "IsDeleted" });
            DropTable("dbo.TblServiceRequest",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ServiceRequestHistory_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
