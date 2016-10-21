namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OperativeSchedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NumberOfTrain = c.Int(nullable: false),
                        RouteName = c.String(),
                        ArrivalTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        DepartureTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Stations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EcpCode = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Stations");
            DropTable("dbo.OperativeSchedules");
        }
    }
}
