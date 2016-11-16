namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeinRegularSheduleTableDaysFollowingstypeMany2Many : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DaysFollowings", "RegulatorySchedule_Id", "dbo.RegulatorySchedules");
            DropIndex("dbo.DaysFollowings", new[] { "RegulatorySchedule_Id" });
            CreateTable(
                "dbo.DaysFollowingsRegulatorySchedules",
                c => new
                    {
                        RegulatorySchedulesId = c.Int(nullable: false),
                        DaysFollowingsId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RegulatorySchedulesId, t.DaysFollowingsId })
                .ForeignKey("dbo.RegulatorySchedules", t => t.RegulatorySchedulesId, cascadeDelete: true)
                .ForeignKey("dbo.DaysFollowings", t => t.DaysFollowingsId, cascadeDelete: true)
                .Index(t => t.RegulatorySchedulesId)
                .Index(t => t.DaysFollowingsId);
            
            AddColumn("dbo.RegulatorySchedules", "RailwayStation_Id", c => c.Int());
            CreateIndex("dbo.RegulatorySchedules", "RailwayStation_Id");
            AddForeignKey("dbo.RegulatorySchedules", "RailwayStation_Id", "dbo.RailwayStations", "Id");
            DropColumn("dbo.DaysFollowings", "RegulatorySchedule_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DaysFollowings", "RegulatorySchedule_Id", c => c.Int());
            DropForeignKey("dbo.RegulatorySchedules", "RailwayStation_Id", "dbo.RailwayStations");
            DropForeignKey("dbo.DaysFollowingsRegulatorySchedules", "DaysFollowingsId", "dbo.DaysFollowings");
            DropForeignKey("dbo.DaysFollowingsRegulatorySchedules", "RegulatorySchedulesId", "dbo.RegulatorySchedules");
            DropIndex("dbo.DaysFollowingsRegulatorySchedules", new[] { "DaysFollowingsId" });
            DropIndex("dbo.DaysFollowingsRegulatorySchedules", new[] { "RegulatorySchedulesId" });
            DropIndex("dbo.RegulatorySchedules", new[] { "RailwayStation_Id" });
            DropColumn("dbo.RegulatorySchedules", "RailwayStation_Id");
            DropTable("dbo.DaysFollowingsRegulatorySchedules");
            CreateIndex("dbo.DaysFollowings", "RegulatorySchedule_Id");
            AddForeignKey("dbo.DaysFollowings", "RegulatorySchedule_Id", "dbo.RegulatorySchedules", "Id");
        }
    }
}
