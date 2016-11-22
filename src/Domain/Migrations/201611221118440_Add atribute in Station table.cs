namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddatributeinStationtable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Stations", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Stations", "Name", c => c.String());
        }
    }
}
