namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFKDispathandDestinStationtable : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.RegulatorySchedules", name: "StationOfDestination_Id", newName: "DestinationStation_Id");
            RenameIndex(table: "dbo.RegulatorySchedules", name: "IX_StationOfDestination_Id", newName: "IX_DestinationStation_Id");
            AddColumn("dbo.Stations", "DestinationRegulatorySchedule_Id", c => c.Int());
            AddColumn("dbo.Stations", "DispatchRegulatorySchedule_Id", c => c.Int());
            CreateIndex("dbo.Stations", "DestinationRegulatorySchedule_Id");
            CreateIndex("dbo.Stations", "DispatchRegulatorySchedule_Id");
            AddForeignKey("dbo.Stations", "DestinationRegulatorySchedule_Id", "dbo.RegulatorySchedules", "Id");
            AddForeignKey("dbo.Stations", "DispatchRegulatorySchedule_Id", "dbo.RegulatorySchedules", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stations", "DispatchRegulatorySchedule_Id", "dbo.RegulatorySchedules");
            DropForeignKey("dbo.Stations", "DestinationRegulatorySchedule_Id", "dbo.RegulatorySchedules");
            DropIndex("dbo.Stations", new[] { "DispatchRegulatorySchedule_Id" });
            DropIndex("dbo.Stations", new[] { "DestinationRegulatorySchedule_Id" });
            DropColumn("dbo.Stations", "DispatchRegulatorySchedule_Id");
            DropColumn("dbo.Stations", "DestinationRegulatorySchedule_Id");
            RenameIndex(table: "dbo.RegulatorySchedules", name: "IX_DestinationStation_Id", newName: "IX_StationOfDestination_Id");
            RenameColumn(table: "dbo.RegulatorySchedules", name: "DestinationStation_Id", newName: "StationOfDestination_Id");
        }
    }
}
