namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDiagnosticsCollectionsInRailwayStationTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Diagnostics", "RailwayStation_Id", c => c.Int());
            CreateIndex("dbo.Diagnostics", "RailwayStation_Id");
            AddForeignKey("dbo.Diagnostics", "RailwayStation_Id", "dbo.RailwayStations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Diagnostics", "RailwayStation_Id", "dbo.RailwayStations");
            DropIndex("dbo.Diagnostics", new[] { "RailwayStation_Id" });
            DropColumn("dbo.Diagnostics", "RailwayStation_Id");
        }
    }
}
