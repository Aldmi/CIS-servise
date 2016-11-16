namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRegularSheduleTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RegulatorySchedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RouteName = c.String(),
                        ArrivalTime = c.DateTime(nullable: false),
                        DepartureTime = c.DateTime(nullable: false),
                        DispatchStation_Id = c.Int(),
                        StationOfDestination_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stations", t => t.DispatchStation_Id)
                .ForeignKey("dbo.Stations", t => t.StationOfDestination_Id)
                .Index(t => t.DispatchStation_Id)
                .Index(t => t.StationOfDestination_Id);
            
            AddColumn("dbo.Stations", "RegulatorySchedule_Id", c => c.Int());
            AddColumn("dbo.Stations", "RegulatorySchedule_Id1", c => c.Int());
            CreateIndex("dbo.Stations", "RegulatorySchedule_Id");
            CreateIndex("dbo.Stations", "RegulatorySchedule_Id1");
            AddForeignKey("dbo.Stations", "RegulatorySchedule_Id", "dbo.RegulatorySchedules", "Id");
            AddForeignKey("dbo.Stations", "RegulatorySchedule_Id1", "dbo.RegulatorySchedules", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RegulatorySchedules", "StationOfDestination_Id", "dbo.Stations");
            DropForeignKey("dbo.Stations", "RegulatorySchedule_Id1", "dbo.RegulatorySchedules");
            DropForeignKey("dbo.Stations", "RegulatorySchedule_Id", "dbo.RegulatorySchedules");
            DropForeignKey("dbo.RegulatorySchedules", "DispatchStation_Id", "dbo.Stations");
            DropIndex("dbo.RegulatorySchedules", new[] { "StationOfDestination_Id" });
            DropIndex("dbo.RegulatorySchedules", new[] { "DispatchStation_Id" });
            DropIndex("dbo.Stations", new[] { "RegulatorySchedule_Id1" });
            DropIndex("dbo.Stations", new[] { "RegulatorySchedule_Id" });
            DropColumn("dbo.Stations", "RegulatorySchedule_Id1");
            DropColumn("dbo.Stations", "RegulatorySchedule_Id");
            DropTable("dbo.RegulatorySchedules");
        }
    }
}
