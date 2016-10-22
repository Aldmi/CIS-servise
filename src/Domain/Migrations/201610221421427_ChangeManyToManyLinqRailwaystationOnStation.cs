namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeManyToManyLinqRailwaystationOnStation : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.RailwayStationsAllStations", newName: "RailwayStationStations");
            RenameColumn(table: "dbo.RailwayStationStations", name: "StationId", newName: "Station_Id");
            RenameColumn(table: "dbo.RailwayStationStations", name: "RailwayStationId", newName: "RailwayStation_Id");
            RenameIndex(table: "dbo.RailwayStationStations", name: "IX_RailwayStationId", newName: "IX_RailwayStation_Id");
            RenameIndex(table: "dbo.RailwayStationStations", name: "IX_StationId", newName: "IX_Station_Id");
            DropPrimaryKey("dbo.RailwayStationStations");
            AddPrimaryKey("dbo.RailwayStationStations", new[] { "RailwayStation_Id", "Station_Id" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.RailwayStationStations");
            AddPrimaryKey("dbo.RailwayStationStations", new[] { "StationId", "RailwayStationId" });
            RenameIndex(table: "dbo.RailwayStationStations", name: "IX_Station_Id", newName: "IX_StationId");
            RenameIndex(table: "dbo.RailwayStationStations", name: "IX_RailwayStation_Id", newName: "IX_RailwayStationId");
            RenameColumn(table: "dbo.RailwayStationStations", name: "RailwayStation_Id", newName: "RailwayStationId");
            RenameColumn(table: "dbo.RailwayStationStations", name: "Station_Id", newName: "StationId");
            RenameTable(name: "dbo.RailwayStationStations", newName: "RailwayStationsAllStations");
        }
    }
}
