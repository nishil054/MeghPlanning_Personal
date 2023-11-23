namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifyUserTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AbpUsers", "Birthdate", c => c.DateTime());
            AlterColumn("dbo.AbpUsers", "Next_Renewaldate", c => c.DateTime());
            AlterColumn("dbo.AbpUsers", "Joiningdate", c => c.DateTime());
            AlterColumn("dbo.AbpUsers", "Resigndate", c => c.DateTime());
            AlterColumn("dbo.AbpUsers", "Lastdate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AbpUsers", "Lastdate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.AbpUsers", "Resigndate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.AbpUsers", "Joiningdate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.AbpUsers", "Next_Renewaldate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.AbpUsers", "Birthdate", c => c.DateTime(nullable: false));
        }
    }
}
