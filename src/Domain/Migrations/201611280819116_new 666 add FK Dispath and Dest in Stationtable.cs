namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class new666addFKDispathandDestinStationtable : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.OperativeSchedules", name: "StationOfDestination_Id", newName: "DestinationStation_Id");
            RenameColumn(table: "dbo.Infoes", name: "StationOfDestination_Id", newName: "DestinationStation_Id");
            RenameIndex(table: "dbo.Infoes", name: "IX_StationOfDestination_Id", newName: "IX_DestinationStation_Id");
            RenameIndex(table: "dbo.OperativeSchedules", name: "IX_StationOfDestination_Id", newName: "IX_DestinationStation_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.OperativeSchedules", name: "IX_DestinationStation_Id", newName: "IX_StationOfDestination_Id");
            RenameIndex(table: "dbo.Infoes", name: "IX_DestinationStation_Id", newName: "IX_StationOfDestination_Id");
            RenameColumn(table: "dbo.Infoes", name: "DestinationStation_Id", newName: "StationOfDestination_Id");
            RenameColumn(table: "dbo.OperativeSchedules", name: "DestinationStation_Id", newName: "StationOfDestination_Id");
        }
    }
}
