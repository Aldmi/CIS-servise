namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFKRailwayStationinRegulatorySh : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RegulatorySchedules", "RailwayStation_Id", "dbo.RailwayStations");
            DropIndex("dbo.RegulatorySchedules", new[] { "RailwayStation_Id" });
            AlterColumn("dbo.RegulatorySchedules", "RailwayStation_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.RegulatorySchedules", "RailwayStation_Id");
            AddForeignKey("dbo.RegulatorySchedules", "RailwayStation_Id", "dbo.RailwayStations", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RegulatorySchedules", "RailwayStation_Id", "dbo.RailwayStations");
            DropIndex("dbo.RegulatorySchedules", new[] { "RailwayStation_Id" });
            AlterColumn("dbo.RegulatorySchedules", "RailwayStation_Id", c => c.Int());
            CreateIndex("dbo.RegulatorySchedules", "RailwayStation_Id");
            AddForeignKey("dbo.RegulatorySchedules", "RailwayStation_Id", "dbo.RailwayStations", "Id");
        }
    }
}
