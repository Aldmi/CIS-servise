namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDispatchStationProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OperativeSchedules", "DispatchStation_Id", c => c.Int());
            CreateIndex("dbo.OperativeSchedules", "DispatchStation_Id");
            AddForeignKey("dbo.OperativeSchedules", "DispatchStation_Id", "dbo.Stations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OperativeSchedules", "DispatchStation_Id", "dbo.Stations");
            DropIndex("dbo.OperativeSchedules", new[] { "DispatchStation_Id" });
            DropColumn("dbo.OperativeSchedules", "DispatchStation_Id");
        }
    }
}
