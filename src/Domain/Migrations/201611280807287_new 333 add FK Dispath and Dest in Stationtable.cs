namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class new333addFKDispathandDestinStationtable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RegulatorySchedules", "Station_Id", c => c.Int());
            AddColumn("dbo.RegulatorySchedules", "Station_Id1", c => c.Int());
            CreateIndex("dbo.RegulatorySchedules", "Station_Id");
            CreateIndex("dbo.RegulatorySchedules", "Station_Id1");
            AddForeignKey("dbo.RegulatorySchedules", "Station_Id", "dbo.Stations", "Id");
            AddForeignKey("dbo.RegulatorySchedules", "Station_Id1", "dbo.Stations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RegulatorySchedules", "Station_Id1", "dbo.Stations");
            DropForeignKey("dbo.RegulatorySchedules", "Station_Id", "dbo.Stations");
            DropIndex("dbo.RegulatorySchedules", new[] { "Station_Id1" });
            DropIndex("dbo.RegulatorySchedules", new[] { "Station_Id" });
            DropColumn("dbo.RegulatorySchedules", "Station_Id1");
            DropColumn("dbo.RegulatorySchedules", "Station_Id");
        }
    }
}
