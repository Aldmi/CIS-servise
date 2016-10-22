namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Test1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RailwayStations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Stations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EcpCode = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StationRailwayStations",
                c => new
                    {
                        Station_Id = c.Int(nullable: false),
                        RailwayStation_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Station_Id, t.RailwayStation_Id })
                .ForeignKey("dbo.Stations", t => t.Station_Id, cascadeDelete: true)
                .ForeignKey("dbo.RailwayStations", t => t.RailwayStation_Id, cascadeDelete: true)
                .Index(t => t.Station_Id)
                .Index(t => t.RailwayStation_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StationRailwayStations", "RailwayStation_Id", "dbo.RailwayStations");
            DropForeignKey("dbo.StationRailwayStations", "Station_Id", "dbo.Stations");
            DropIndex("dbo.StationRailwayStations", new[] { "RailwayStation_Id" });
            DropIndex("dbo.StationRailwayStations", new[] { "Station_Id" });
            DropTable("dbo.StationRailwayStations");
            DropTable("dbo.Stations");
            DropTable("dbo.RailwayStations");
        }
    }
}
