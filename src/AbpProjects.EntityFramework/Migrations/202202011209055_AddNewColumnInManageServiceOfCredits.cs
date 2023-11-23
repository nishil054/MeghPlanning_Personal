namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewColumnInManageServiceOfCredits : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TblManageService", "Credits", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TblManageService", "Credits");
        }
    }
}
