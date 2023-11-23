﻿namespace AbpProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_NewColumn_EffortsInTimesheet : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TimeSheet", "Efforts", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TimeSheet", "Efforts");
        }
    }
}
