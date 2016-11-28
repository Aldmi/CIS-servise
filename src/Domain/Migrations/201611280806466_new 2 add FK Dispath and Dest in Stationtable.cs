namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class new2addFKDispathandDestinStationtable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Stations", "DestinationRegulatorySchedule_Id", "dbo.RegulatorySchedules");
            DropForeignKey("dbo.Stations", "DispatchRegulatorySchedule_Id", "dbo.RegulatorySchedules");
            DropIndex("dbo.Stations", new[] { "DestinationRegulatorySchedule_Id" });
            DropIndex("dbo.Stations", new[] { "DispatchRegulatorySchedule_Id" });
            DropColumn("dbo.Stations", "DestinationRegulatorySchedule_Id");
            DropColumn("dbo.Stations", "DispatchRegulatorySchedule_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Stations", "DispatchRegulatorySchedule_Id", c => c.Int());
            AddColumn("dbo.Stations", "DestinationRegulatorySchedule_Id", c => c.Int());
            CreateIndex("dbo.Stations", "DispatchRegulatorySchedule_Id");
            CreateIndex("dbo.Stations", "DestinationRegulatorySchedule_Id");
            AddForeignKey("dbo.Stations", "DispatchRegulatorySchedule_Id", "dbo.RegulatorySchedules", "Id");
            AddForeignKey("dbo.Stations", "DestinationRegulatorySchedule_Id", "dbo.RegulatorySchedules", "Id");
        }
    }
}
