namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOperativeSchedule : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.StationRailwayStations", newName: "RailwayStationStations");
            DropPrimaryKey("dbo.RailwayStationStations");
            CreateTable(
                "dbo.OperativeSchedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NumberOfTrain = c.Int(nullable: false),
                        RouteName = c.String(maxLength: 100),
                        ArrivalTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        DepartureTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        RailwayStation_Id = c.Int(),
                        DispatchStation_Id = c.Int(),
                        StationOfDestination_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RailwayStations", t => t.RailwayStation_Id)
                .ForeignKey("dbo.Stations", t => t.DispatchStation_Id)
                .ForeignKey("dbo.Stations", t => t.StationOfDestination_Id)
                .Index(t => t.RailwayStation_Id)
                .Index(t => t.DispatchStation_Id)
                .Index(t => t.StationOfDestination_Id);
            
            CreateTable(
                "dbo.OperativeSchedulesListOfStops",
                c => new
                    {
                        StationId = c.Int(nullable: false),
                        OperativeSchedulesId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StationId, t.OperativeSchedulesId })
                .ForeignKey("dbo.Stations", t => t.StationId, cascadeDelete: true)
                .ForeignKey("dbo.OperativeSchedules", t => t.OperativeSchedulesId, cascadeDelete: true)
                .Index(t => t.StationId)
                .Index(t => t.OperativeSchedulesId);
            
            CreateTable(
                "dbo.OperativeSchedulesListWithoutStops",
                c => new
                    {
                        StationId = c.Int(nullable: false),
                        OperativeSchedulesId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StationId, t.OperativeSchedulesId })
                .ForeignKey("dbo.Stations", t => t.StationId, cascadeDelete: true)
                .ForeignKey("dbo.OperativeSchedules", t => t.OperativeSchedulesId, cascadeDelete: true)
                .Index(t => t.StationId)
                .Index(t => t.OperativeSchedulesId);
            
            AddPrimaryKey("dbo.RailwayStationStations", new[] { "RailwayStation_Id", "Station_Id" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OperativeSchedules", "StationOfDestination_Id", "dbo.Stations");
            DropForeignKey("dbo.OperativeSchedules", "DispatchStation_Id", "dbo.Stations");
            DropForeignKey("dbo.OperativeSchedules", "RailwayStation_Id", "dbo.RailwayStations");
            DropForeignKey("dbo.OperativeSchedulesListWithoutStops", "OperativeSchedulesId", "dbo.OperativeSchedules");
            DropForeignKey("dbo.OperativeSchedulesListWithoutStops", "StationId", "dbo.Stations");
            DropForeignKey("dbo.OperativeSchedulesListOfStops", "OperativeSchedulesId", "dbo.OperativeSchedules");
            DropForeignKey("dbo.OperativeSchedulesListOfStops", "StationId", "dbo.Stations");
            DropIndex("dbo.OperativeSchedulesListWithoutStops", new[] { "OperativeSchedulesId" });
            DropIndex("dbo.OperativeSchedulesListWithoutStops", new[] { "StationId" });
            DropIndex("dbo.OperativeSchedulesListOfStops", new[] { "OperativeSchedulesId" });
            DropIndex("dbo.OperativeSchedulesListOfStops", new[] { "StationId" });
            DropIndex("dbo.OperativeSchedules", new[] { "StationOfDestination_Id" });
            DropIndex("dbo.OperativeSchedules", new[] { "DispatchStation_Id" });
            DropIndex("dbo.OperativeSchedules", new[] { "RailwayStation_Id" });
            DropPrimaryKey("dbo.RailwayStationStations");
            DropTable("dbo.OperativeSchedulesListWithoutStops");
            DropTable("dbo.OperativeSchedulesListOfStops");
            DropTable("dbo.OperativeSchedules");
            AddPrimaryKey("dbo.RailwayStationStations", new[] { "Station_Id", "RailwayStation_Id" });
            RenameTable(name: "dbo.RailwayStationStations", newName: "StationRailwayStations");
        }
    }
}
