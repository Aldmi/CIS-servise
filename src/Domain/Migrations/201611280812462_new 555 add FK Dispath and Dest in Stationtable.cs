namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class new555addFKDispathandDestinStationtable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RegulatorySchedules", "Station_Id", "dbo.Stations");
            DropForeignKey("dbo.RegulatorySchedules", "Station_Id1", "dbo.Stations");
            DropIndex("dbo.RegulatorySchedules", new[] { "Station_Id" });
            DropIndex("dbo.RegulatorySchedules", new[] { "Station_Id1" });
            DropColumn("dbo.RegulatorySchedules", "Station_Id");
            DropColumn("dbo.RegulatorySchedules", "Station_Id1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RegulatorySchedules", "Station_Id1", c => c.Int());
            AddColumn("dbo.RegulatorySchedules", "Station_Id", c => c.Int());
            CreateIndex("dbo.RegulatorySchedules", "Station_Id1");
            CreateIndex("dbo.RegulatorySchedules", "Station_Id");
            AddForeignKey("dbo.RegulatorySchedules", "Station_Id1", "dbo.Stations", "Id");
            AddForeignKey("dbo.RegulatorySchedules", "Station_Id", "dbo.Stations", "Id");
        }
    }
}
