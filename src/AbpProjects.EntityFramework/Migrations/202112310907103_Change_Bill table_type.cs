namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_Billtable_type : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bill", "LastModificationTime", c => c.DateTime());
            AddColumn("dbo.Bill", "LastModifierUserId", c => c.Long());
            AddColumn("dbo.Bill", "CreationTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Bill", "CreatorUserId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bill", "CreatorUserId");
            DropColumn("dbo.Bill", "CreationTime");
            DropColumn("dbo.Bill", "LastModifierUserId");
            DropColumn("dbo.Bill", "LastModificationTime");
        }
    }
}
