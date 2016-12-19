namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateTimesetnullableinarrivalTimeandDepartureTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Infoes", "RailwayStation_Id", c => c.Int());
            AlterColumn("dbo.Infoes", "ArrivalTime", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Infoes", "DepartureTime", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Infoes", "RouteName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.OperativeSchedules", "ArrivalTime", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.OperativeSchedules", "DepartureTime", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.RailwayStations", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.RegulatorySchedules", "ArrivalTime", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.RegulatorySchedules", "DepartureTime", c => c.DateTime(precision: 7, storeType: "datetime2"));
            CreateIndex("dbo.Infoes", "RailwayStation_Id");
            AddForeignKey("dbo.Infoes", "RailwayStation_Id", "dbo.RailwayStations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Infoes", "RailwayStation_Id", "dbo.RailwayStations");
            DropIndex("dbo.Infoes", new[] { "RailwayStation_Id" });
            AlterColumn("dbo.RegulatorySchedules", "DepartureTime", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.RegulatorySchedules", "ArrivalTime", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.RailwayStations", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.OperativeSchedules", "DepartureTime", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.OperativeSchedules", "ArrivalTime", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Infoes", "RouteName", c => c.String());
            AlterColumn("dbo.Infoes", "DepartureTime", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Infoes", "ArrivalTime", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            DropColumn("dbo.Infoes", "RailwayStation_Id");
        }
    }
}
