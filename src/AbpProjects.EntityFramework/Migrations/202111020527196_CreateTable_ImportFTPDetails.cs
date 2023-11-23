namespace AbpProjects.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTable_ImportFTPDetails : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ImportFTPDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DomainName = c.String(),
                        HostName = c.String(),
                        FTPUserName = c.String(),
                        FTPPassword = c.String(),
                        DBType = c.String(),
                        OnlineManager = c.String(),
                        OnlineManagerHostName = c.String(),
                        DatabaseName = c.String(),
                        DataBaseUserName = c.String(),
                        DataBasePassword = c.String(),
                        Storagecontainer = c.String(),
                        MailProvider_Host = c.String(),
                        MailProvider_User = c.String(),
                        MailProvider_Password = c.String(),
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
                    { "DynamicFilter_ImportFTPDetails_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.IsDeleted);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ImportFTPDetails", new[] { "IsDeleted" });
            DropTable("dbo.ImportFTPDetails",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ImportFTPDetails_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
