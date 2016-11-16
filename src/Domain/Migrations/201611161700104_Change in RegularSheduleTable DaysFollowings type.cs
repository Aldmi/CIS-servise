namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeinRegularSheduleTableDaysFollowingstype : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DaysFollowings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateTimeDb = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        RegulatorySchedule_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RegulatorySchedules", t => t.RegulatorySchedule_Id)
                .Index(t => t.RegulatorySchedule_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DaysFollowings", "RegulatorySchedule_Id", "dbo.RegulatorySchedules");
            DropIndex("dbo.DaysFollowings", new[] { "RegulatorySchedule_Id" });
            DropTable("dbo.DaysFollowings");
        }
    }
}
