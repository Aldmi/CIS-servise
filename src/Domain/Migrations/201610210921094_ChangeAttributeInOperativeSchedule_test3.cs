namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeAttributeInOperativeSchedule_test3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Stations", "OperativeSchedule_Id", "dbo.OperativeSchedules");
            DropForeignKey("dbo.Stations", "OperativeSchedule_Id1", "dbo.OperativeSchedules");
            DropIndex("dbo.Stations", new[] { "OperativeSchedule_Id" });
            DropIndex("dbo.Stations", new[] { "OperativeSchedule_Id1" });
            DropColumn("dbo.Stations", "OperativeSchedule_Id");
            DropColumn("dbo.Stations", "OperativeSchedule_Id1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Stations", "OperativeSchedule_Id1", c => c.Int());
            AddColumn("dbo.Stations", "OperativeSchedule_Id", c => c.Int());
            CreateIndex("dbo.Stations", "OperativeSchedule_Id1");
            CreateIndex("dbo.Stations", "OperativeSchedule_Id");
            AddForeignKey("dbo.Stations", "OperativeSchedule_Id1", "dbo.OperativeSchedules", "Id");
            AddForeignKey("dbo.Stations", "OperativeSchedule_Id", "dbo.OperativeSchedules", "Id");
        }
    }
}
