namespace AbpProjects.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class tblmanageservices : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TblManageService",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServiceId = c.Int(nullable: false),
                        ClientId = c.Int(nullable: false),
                        DomainName = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NextRenewalDate = c.DateTime(nullable: false),
                        Comment = c.String(),
                        EmployeeId = c.Int(nullable: false),
                        HostingSpace = c.String(),
                        ServerType = c.Int(),
                        TypeName = c.String(),
                        NoOfEmail = c.Int(),
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
                    { "DynamicFilter_ManageService_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.IsDeleted);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.TblManageService", new[] { "IsDeleted" });
            DropTable("dbo.TblManageService",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ManageService_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
