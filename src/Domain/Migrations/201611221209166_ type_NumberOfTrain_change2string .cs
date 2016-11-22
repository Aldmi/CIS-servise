namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class type_NumberOfTrain_change2string : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OperativeSchedules", "NumberOfTrain", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.OperativeSchedules", "RouteName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.RegulatorySchedules", "NumberOfTrain", c => c.String(nullable: false, maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RegulatorySchedules", "NumberOfTrain", c => c.Int(nullable: false));
            AlterColumn("dbo.OperativeSchedules", "RouteName", c => c.String(maxLength: 100));
            AlterColumn("dbo.OperativeSchedules", "NumberOfTrain", c => c.Int(nullable: false));
        }
    }
}
