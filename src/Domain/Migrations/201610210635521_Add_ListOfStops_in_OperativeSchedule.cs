namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ListOfStops_in_OperativeSchedule : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stations", "OperativeSchedule_Id", c => c.Int());
            CreateIndex("dbo.Stations", "OperativeSchedule_Id");
            AddForeignKey("dbo.Stations", "OperativeSchedule_Id", "dbo.OperativeSchedules", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stations", "OperativeSchedule_Id", "dbo.OperativeSchedules");
            DropIndex("dbo.Stations", new[] { "OperativeSchedule_Id" });
            DropColumn("dbo.Stations", "OperativeSchedule_Id");
        }
    }
}
