namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change_titlewidth_document : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Document", "Title", c => c.String(nullable: false, maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Document", "Title", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
