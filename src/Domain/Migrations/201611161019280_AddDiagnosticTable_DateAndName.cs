namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDiagnosticTable_DateAndName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Diagnostics", "DeviceName", c => c.String());
            AddColumn("dbo.Diagnostics", "Date", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Diagnostics", "Date");
            DropColumn("dbo.Diagnostics", "DeviceName");
        }
    }
}
