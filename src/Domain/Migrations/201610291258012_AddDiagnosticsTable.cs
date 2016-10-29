namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDiagnosticsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Diagnostics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DeviceNumber = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Fault = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Diagnostics");
        }
    }
}
