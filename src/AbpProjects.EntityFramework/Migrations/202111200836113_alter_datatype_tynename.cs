namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alter_datatype_tynename : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TblManageService", "TypeName", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TblManageService", "TypeName", c => c.String());
        }
    }
}
