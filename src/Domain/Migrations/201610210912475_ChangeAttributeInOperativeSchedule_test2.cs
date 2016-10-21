namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeAttributeInOperativeSchedule_test2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OperativeSchedules", "StationOfDestination_Id", c => c.Int());
            AddColumn("dbo.Stations", "OperativeSchedule_Id1", c => c.Int());
            AlterColumn("dbo.OperativeSchedules", "RouteName", c => c.String(maxLength: 100));
            CreateIndex("dbo.OperativeSchedules", "StationOfDestination_Id");
            CreateIndex("dbo.Stations", "OperativeSchedule_Id1");
            AddForeignKey("dbo.Stations", "OperativeSchedule_Id1", "dbo.OperativeSchedules", "Id");
            AddForeignKey("dbo.OperativeSchedules", "StationOfDestination_Id", "dbo.Stations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OperativeSchedules", "StationOfDestination_Id", "dbo.Stations");
            DropForeignKey("dbo.Stations", "OperativeSchedule_Id1", "dbo.OperativeSchedules");
            DropIndex("dbo.Stations", new[] { "OperativeSchedule_Id1" });
            DropIndex("dbo.OperativeSchedules", new[] { "StationOfDestination_Id" });
            AlterColumn("dbo.OperativeSchedules", "RouteName", c => c.String());
            DropColumn("dbo.Stations", "OperativeSchedule_Id1");
            DropColumn("dbo.OperativeSchedules", "StationOfDestination_Id");
        }
    }
}
