namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RegulatoryShedulesTableNumberOfTrainChangetypeuint2int : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RegulatorySchedules", "NumberOfTrain", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RegulatorySchedules", "NumberOfTrain");
        }
    }
}
