namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFKRailwayStationinOperativeSh : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OperativeSchedules", "RailwayStation_Id", "dbo.RailwayStations");
            DropIndex("dbo.OperativeSchedules", new[] { "RailwayStation_Id" });
            AlterColumn("dbo.OperativeSchedules", "RailwayStation_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.OperativeSchedules", "RailwayStation_Id");
            AddForeignKey("dbo.OperativeSchedules", "RailwayStation_Id", "dbo.RailwayStations", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OperativeSchedules", "RailwayStation_Id", "dbo.RailwayStations");
            DropIndex("dbo.OperativeSchedules", new[] { "RailwayStation_Id" });
            AlterColumn("dbo.OperativeSchedules", "RailwayStation_Id", c => c.Int());
            CreateIndex("dbo.OperativeSchedules", "RailwayStation_Id");
            AddForeignKey("dbo.OperativeSchedules", "RailwayStation_Id", "dbo.RailwayStations", "Id");
        }
    }
}
