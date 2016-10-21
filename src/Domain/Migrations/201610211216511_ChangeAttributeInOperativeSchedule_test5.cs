namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeAttributeInOperativeSchedule_test5 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OperativeSchedulesListWithoutStops",
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
            DropForeignKey("dbo.OperativeSchedulesListWithoutStops", "StationId", "dbo.OperativeSchedules");
            DropForeignKey("dbo.OperativeSchedulesListWithoutStops", "OperativeSchedulesId", "dbo.Stations");
            DropIndex("dbo.OperativeSchedulesListWithoutStops", new[] { "StationId" });
            DropIndex("dbo.OperativeSchedulesListWithoutStops", new[] { "OperativeSchedulesId" });
            DropTable("dbo.OperativeSchedulesListWithoutStops");
        }
    }
}
