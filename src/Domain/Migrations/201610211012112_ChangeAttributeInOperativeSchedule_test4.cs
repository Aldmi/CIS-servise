namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeAttributeInOperativeSchedule_test4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OperativeSchedulesListOfStops",
                c => new
                    {
                        OperativeSchedulesId = c.Int(nullable: false),
                        StationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.OperativeSchedulesId, t.StationId })
                .ForeignKey("dbo.Stations", t => t.OperativeSchedulesId, cascadeDelete: true)
                .ForeignKey("dbo.OperativeSchedules", t => t.StationId, cascadeDelete: true)
                .Index(t => t.OperativeSchedulesId)
                .Index(t => t.StationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OperativeSchedulesListOfStops", "StationId", "dbo.OperativeSchedules");
            DropForeignKey("dbo.OperativeSchedulesListOfStops", "OperativeSchedulesId", "dbo.Stations");
            DropIndex("dbo.OperativeSchedulesListOfStops", new[] { "StationId" });
            DropIndex("dbo.OperativeSchedulesListOfStops", new[] { "OperativeSchedulesId" });
            DropTable("dbo.OperativeSchedulesListOfStops");
        }
    }
}
