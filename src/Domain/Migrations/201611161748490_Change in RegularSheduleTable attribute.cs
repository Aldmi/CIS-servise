namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeinRegularSheduleTableattribute : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Stations", "RegulatorySchedule_Id", "dbo.RegulatorySchedules");
            DropForeignKey("dbo.Stations", "RegulatorySchedule_Id1", "dbo.RegulatorySchedules");
            DropIndex("dbo.Stations", new[] { "RegulatorySchedule_Id" });
            DropIndex("dbo.Stations", new[] { "RegulatorySchedule_Id1" });
            CreateTable(
                "dbo.RegulatorySchedulesListOfStops",
                c => new
                    {
                        StationId = c.Int(nullable: false),
                        RegulatorySchedulesId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StationId, t.RegulatorySchedulesId })
                .ForeignKey("dbo.Stations", t => t.StationId, cascadeDelete: true)
                .ForeignKey("dbo.RegulatorySchedules", t => t.RegulatorySchedulesId, cascadeDelete: true)
                .Index(t => t.StationId)
                .Index(t => t.RegulatorySchedulesId);
            
            CreateTable(
                "dbo.RegulatorySchedulesListWithoutStops",
                c => new
                    {
                        StationId = c.Int(nullable: false),
                        RegulatorySchedulesId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StationId, t.RegulatorySchedulesId })
                .ForeignKey("dbo.Stations", t => t.StationId, cascadeDelete: true)
                .ForeignKey("dbo.RegulatorySchedules", t => t.RegulatorySchedulesId, cascadeDelete: true)
                .Index(t => t.StationId)
                .Index(t => t.RegulatorySchedulesId);
            
            DropColumn("dbo.Stations", "RegulatorySchedule_Id");
            DropColumn("dbo.Stations", "RegulatorySchedule_Id1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Stations", "RegulatorySchedule_Id1", c => c.Int());
            AddColumn("dbo.Stations", "RegulatorySchedule_Id", c => c.Int());
            DropForeignKey("dbo.RegulatorySchedulesListWithoutStops", "RegulatorySchedulesId", "dbo.RegulatorySchedules");
            DropForeignKey("dbo.RegulatorySchedulesListWithoutStops", "StationId", "dbo.Stations");
            DropForeignKey("dbo.RegulatorySchedulesListOfStops", "RegulatorySchedulesId", "dbo.RegulatorySchedules");
            DropForeignKey("dbo.RegulatorySchedulesListOfStops", "StationId", "dbo.Stations");
            DropIndex("dbo.RegulatorySchedulesListWithoutStops", new[] { "RegulatorySchedulesId" });
            DropIndex("dbo.RegulatorySchedulesListWithoutStops", new[] { "StationId" });
            DropIndex("dbo.RegulatorySchedulesListOfStops", new[] { "RegulatorySchedulesId" });
            DropIndex("dbo.RegulatorySchedulesListOfStops", new[] { "StationId" });
            DropTable("dbo.RegulatorySchedulesListWithoutStops");
            DropTable("dbo.RegulatorySchedulesListOfStops");
            CreateIndex("dbo.Stations", "RegulatorySchedule_Id1");
            CreateIndex("dbo.Stations", "RegulatorySchedule_Id");
            AddForeignKey("dbo.Stations", "RegulatorySchedule_Id1", "dbo.RegulatorySchedules", "Id");
            AddForeignKey("dbo.Stations", "RegulatorySchedule_Id", "dbo.RegulatorySchedules", "Id");
        }
    }
}
