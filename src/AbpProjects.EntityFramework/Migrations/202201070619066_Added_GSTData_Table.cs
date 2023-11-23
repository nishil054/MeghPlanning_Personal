namespace AbpProjects.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Added_GSTData_Table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GSTData",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Month = c.String(),
                        OutputGST = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InputGST = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalPayableGST = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalPendingPayment = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CompanyId = c.Int(nullable: false),
                        FinancialyearId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        MonthId = c.Int(nullable: false),
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
                    { "DynamicFilter_gstDashboard_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.IsDeleted);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.GSTData", new[] { "IsDeleted" });
            DropTable("dbo.GSTData",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_gstDashboard_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
