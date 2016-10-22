namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInfoAndDiagnosticTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Infoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ArrivalTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        DepartureTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Platform = c.Int(nullable: false),
                        Way = c.Int(nullable: false),
                        RouteName = c.String(),
                        Lateness = c.Int(nullable: false),
                        DispatchStation_Id = c.Int(),
                        StationOfDestination_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stations", t => t.DispatchStation_Id)
                .ForeignKey("dbo.Stations", t => t.StationOfDestination_Id)
                .Index(t => t.DispatchStation_Id)
                .Index(t => t.StationOfDestination_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Infoes", "StationOfDestination_Id", "dbo.Stations");
            DropForeignKey("dbo.Infoes", "DispatchStation_Id", "dbo.Stations");
            DropIndex("dbo.Infoes", new[] { "StationOfDestination_Id" });
            DropIndex("dbo.Infoes", new[] { "DispatchStation_Id" });
            DropTable("dbo.Infoes");
        }
    }
}
