namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeinRegularSheduleTableDaysFollowingstypeonstring : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DaysFollowingsRegulatorySchedules", "RegulatorySchedulesId", "dbo.RegulatorySchedules");
            DropForeignKey("dbo.DaysFollowingsRegulatorySchedules", "DaysFollowingsId", "dbo.DaysFollowings");
            DropIndex("dbo.DaysFollowingsRegulatorySchedules", new[] { "RegulatorySchedulesId" });
            DropIndex("dbo.DaysFollowingsRegulatorySchedules", new[] { "DaysFollowingsId" });
            AddColumn("dbo.RegulatorySchedules", "DaysFollowings", c => c.String());
            AlterColumn("dbo.RegulatorySchedules", "RouteName", c => c.String(maxLength: 100));
            AlterColumn("dbo.RegulatorySchedules", "ArrivalTime", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.RegulatorySchedules", "DepartureTime", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            DropTable("dbo.DaysFollowings");
            DropTable("dbo.DaysFollowingsRegulatorySchedules");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DaysFollowingsRegulatorySchedules",
                c => new
                    {
                        RegulatorySchedulesId = c.Int(nullable: false),
                        DaysFollowingsId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RegulatorySchedulesId, t.DaysFollowingsId });
            
            CreateTable(
                "dbo.DaysFollowings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateTimeDb = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.RegulatorySchedules", "DepartureTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.RegulatorySchedules", "ArrivalTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.RegulatorySchedules", "RouteName", c => c.String());
            DropColumn("dbo.RegulatorySchedules", "DaysFollowings");
            CreateIndex("dbo.DaysFollowingsRegulatorySchedules", "DaysFollowingsId");
            CreateIndex("dbo.DaysFollowingsRegulatorySchedules", "RegulatorySchedulesId");
            AddForeignKey("dbo.DaysFollowingsRegulatorySchedules", "DaysFollowingsId", "dbo.DaysFollowings", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DaysFollowingsRegulatorySchedules", "RegulatorySchedulesId", "dbo.RegulatorySchedules", "Id", cascadeDelete: true);
        }
    }
}
