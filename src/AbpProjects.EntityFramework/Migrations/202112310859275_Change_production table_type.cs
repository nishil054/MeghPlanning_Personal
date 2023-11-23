namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_productiontable_type : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Productions", "LastModificationTime", c => c.DateTime());
            AddColumn("dbo.Productions", "LastModifierUserId", c => c.Long());
            AddColumn("dbo.Productions", "CreationTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Productions", "CreatorUserId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Productions", "CreatorUserId");
            DropColumn("dbo.Productions", "CreationTime");
            DropColumn("dbo.Productions", "LastModifierUserId");
            DropColumn("dbo.Productions", "LastModificationTime");
        }
    }
}
