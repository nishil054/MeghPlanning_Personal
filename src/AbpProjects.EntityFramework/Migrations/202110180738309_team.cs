namespace AbpProjects.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class team : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Team",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TeamName = c.String(nullable: false),
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
                    { "DynamicFilter_team_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.IsDeleted);
            
            AddColumn("dbo.AbpUsers", "CompanyId", c => c.Int());
            AddColumn("dbo.AbpUsers", "Salary_Hour", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.AbpUsers", "Salary_Month", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.AbpUsers", "Birthdate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AbpUsers", "Next_Renewaldate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AbpUsers", "Joiningdate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AbpUsers", "Immediate_supervisorId", c => c.Int());
            AddColumn("dbo.AbpUsers", "TeamId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropIndex("dbo.Team", new[] { "IsDeleted" });
            DropColumn("dbo.AbpUsers", "TeamId");
            DropColumn("dbo.AbpUsers", "Immediate_supervisorId");
            DropColumn("dbo.AbpUsers", "Joiningdate");
            DropColumn("dbo.AbpUsers", "Next_Renewaldate");
            DropColumn("dbo.AbpUsers", "Birthdate");
            DropColumn("dbo.AbpUsers", "Salary_Month");
            DropColumn("dbo.AbpUsers", "Salary_Hour");
            DropColumn("dbo.AbpUsers", "CompanyId");
            DropTable("dbo.Team",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_team_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
