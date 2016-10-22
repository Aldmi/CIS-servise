namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRailwayStationInReposutory : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.OperativeSchedulesListOfStops", name: "OperativeSchedulesId", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.OperativeSchedulesListOfStops", name: "StationId", newName: "OperativeSchedulesId");
            RenameColumn(table: "dbo.OperativeSchedulesListOfStops", name: "__mig_tmp__0", newName: "StationId");
            RenameIndex(table: "dbo.OperativeSchedulesListOfStops", name: "IX_OperativeSchedulesId", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.OperativeSchedulesListOfStops", name: "IX_StationId", newName: "IX_OperativeSchedulesId");
            RenameIndex(table: "dbo.OperativeSchedulesListOfStops", name: "__mig_tmp__0", newName: "IX_StationId");
            CreateTable(
                "dbo.RailwayStations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Diagnostic_Id = c.Int(),
                        Info_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Diagnostics", t => t.Diagnostic_Id)
                .ForeignKey("dbo.Infoes", t => t.Info_Id)
                .Index(t => t.Diagnostic_Id)
                .Index(t => t.Info_Id);
            
            CreateTable(
                "dbo.Diagnostics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DeviceId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Fault = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Infoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ArrivalTime = c.DateTime(nullable: false),
                        DepartureTime = c.DateTime(nullable: false),
                        Platform = c.Int(nullable: false),
                        Way = c.Int(nullable: false),
                        RouteName = c.String(),
                        Lateness = c.Int(nullable: false),
                        DispatchStation_Id = c.Int(),
                        StationOfDestination_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stations", t => t.DispatchStation_Id)
                .ForeignKey("dbo.Stations", t => t.StationOfDestination_Id)
                .Index(t => t.DispatchStation_Id)
                .Index(t => t.StationOfDestination_Id);
            
            CreateTable(
                "dbo.RegulatorySchedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RouteName = c.String(),
                        ArrivalTime = c.DateTime(nullable: false),
                        DepartureTime = c.DateTime(nullable: false),
                        DaysFollowing = c.String(),
                        DispatchStation_Id = c.Int(),
                        StationOfDestination_Id = c.Int(),
                        RailwayStation_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stations", t => t.DispatchStation_Id)
                .ForeignKey("dbo.Stations", t => t.StationOfDestination_Id)
                .ForeignKey("dbo.RailwayStations", t => t.RailwayStation_Id)
                .Index(t => t.DispatchStation_Id)
                .Index(t => t.StationOfDestination_Id)
                .Index(t => t.RailwayStation_Id);
            
            CreateTable(
                "dbo.RailwayStationsAllStations",
                c => new
                    {
                        StationId = c.Int(nullable: false),
                        RailwayStationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StationId, t.RailwayStationId })
                .ForeignKey("dbo.Stations", t => t.StationId, cascadeDelete: true)
                .ForeignKey("dbo.RailwayStations", t => t.RailwayStationId, cascadeDelete: true)
                .Index(t => t.StationId)
                .Index(t => t.RailwayStationId);
            
            AddColumn("dbo.OperativeSchedules", "RailwayStation_Id", c => c.Int());
            AddColumn("dbo.Stations", "RegulatorySchedule_Id", c => c.Int());
            AddColumn("dbo.Stations", "RegulatorySchedule_Id1", c => c.Int());
            CreateIndex("dbo.OperativeSchedules", "RailwayStation_Id");
            CreateIndex("dbo.Stations", "RegulatorySchedule_Id");
            CreateIndex("dbo.Stations", "RegulatorySchedule_Id1");
            AddForeignKey("dbo.OperativeSchedules", "RailwayStation_Id", "dbo.RailwayStations", "Id");
            AddForeignKey("dbo.Stations", "RegulatorySchedule_Id", "dbo.RegulatorySchedules", "Id");
            AddForeignKey("dbo.Stations", "RegulatorySchedule_Id1", "dbo.RegulatorySchedules", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RailwayStationsAllStations", "RailwayStationId", "dbo.RailwayStations");
            DropForeignKey("dbo.RailwayStationsAllStations", "StationId", "dbo.Stations");
            DropForeignKey("dbo.RegulatorySchedules", "RailwayStation_Id", "dbo.RailwayStations");
            DropForeignKey("dbo.RegulatorySchedules", "StationOfDestination_Id", "dbo.Stations");
            DropForeignKey("dbo.Stations", "RegulatorySchedule_Id1", "dbo.RegulatorySchedules");
            DropForeignKey("dbo.Stations", "RegulatorySchedule_Id", "dbo.RegulatorySchedules");
            DropForeignKey("dbo.RegulatorySchedules", "DispatchStation_Id", "dbo.Stations");
            DropForeignKey("dbo.OperativeSchedules", "RailwayStation_Id", "dbo.RailwayStations");
            DropForeignKey("dbo.RailwayStations", "Info_Id", "dbo.Infoes");
            DropForeignKey("dbo.Infoes", "StationOfDestination_Id", "dbo.Stations");
            DropForeignKey("dbo.Infoes", "DispatchStation_Id", "dbo.Stations");
            DropForeignKey("dbo.RailwayStations", "Diagnostic_Id", "dbo.Diagnostics");
            DropIndex("dbo.RailwayStationsAllStations", new[] { "RailwayStationId" });
            DropIndex("dbo.RailwayStationsAllStations", new[] { "StationId" });
            DropIndex("dbo.RegulatorySchedules", new[] { "RailwayStation_Id" });
            DropIndex("dbo.RegulatorySchedules", new[] { "StationOfDestination_Id" });
            DropIndex("dbo.RegulatorySchedules", new[] { "DispatchStation_Id" });
            DropIndex("dbo.Infoes", new[] { "StationOfDestination_Id" });
            DropIndex("dbo.Infoes", new[] { "DispatchStation_Id" });
            DropIndex("dbo.RailwayStations", new[] { "Info_Id" });
            DropIndex("dbo.RailwayStations", new[] { "Diagnostic_Id" });
            DropIndex("dbo.Stations", new[] { "RegulatorySchedule_Id1" });
            DropIndex("dbo.Stations", new[] { "RegulatorySchedule_Id" });
            DropIndex("dbo.OperativeSchedules", new[] { "RailwayStation_Id" });
            DropColumn("dbo.Stations", "RegulatorySchedule_Id1");
            DropColumn("dbo.Stations", "RegulatorySchedule_Id");
            DropColumn("dbo.OperativeSchedules", "RailwayStation_Id");
            DropTable("dbo.RailwayStationsAllStations");
            DropTable("dbo.RegulatorySchedules");
            DropTable("dbo.Infoes");
            DropTable("dbo.Diagnostics");
            DropTable("dbo.RailwayStations");
            RenameIndex(table: "dbo.OperativeSchedulesListOfStops", name: "IX_StationId", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.OperativeSchedulesListOfStops", name: "IX_OperativeSchedulesId", newName: "IX_StationId");
            RenameIndex(table: "dbo.OperativeSchedulesListOfStops", name: "__mig_tmp__0", newName: "IX_OperativeSchedulesId");
            RenameColumn(table: "dbo.OperativeSchedulesListOfStops", name: "StationId", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.OperativeSchedulesListOfStops", name: "OperativeSchedulesId", newName: "StationId");
            RenameColumn(table: "dbo.OperativeSchedulesListOfStops", name: "__mig_tmp__0", newName: "OperativeSchedulesId");
        }
    }
}
