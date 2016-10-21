namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeAttributeInOperativeSchedule_test6 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.OperativeSchedulesListWithoutStops", name: "OperativeSchedulesId", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.OperativeSchedulesListWithoutStops", name: "StationId", newName: "OperativeSchedulesId");
            RenameColumn(table: "dbo.OperativeSchedulesListWithoutStops", name: "__mig_tmp__0", newName: "StationId");
            RenameIndex(table: "dbo.OperativeSchedulesListWithoutStops", name: "IX_OperativeSchedulesId", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.OperativeSchedulesListWithoutStops", name: "IX_StationId", newName: "IX_OperativeSchedulesId");
            RenameIndex(table: "dbo.OperativeSchedulesListWithoutStops", name: "__mig_tmp__0", newName: "IX_StationId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.OperativeSchedulesListWithoutStops", name: "IX_StationId", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.OperativeSchedulesListWithoutStops", name: "IX_OperativeSchedulesId", newName: "IX_StationId");
            RenameIndex(table: "dbo.OperativeSchedulesListWithoutStops", name: "__mig_tmp__0", newName: "IX_OperativeSchedulesId");
            RenameColumn(table: "dbo.OperativeSchedulesListWithoutStops", name: "StationId", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.OperativeSchedulesListWithoutStops", name: "OperativeSchedulesId", newName: "StationId");
            RenameColumn(table: "dbo.OperativeSchedulesListWithoutStops", name: "__mig_tmp__0", newName: "OperativeSchedulesId");
        }
    }
}
