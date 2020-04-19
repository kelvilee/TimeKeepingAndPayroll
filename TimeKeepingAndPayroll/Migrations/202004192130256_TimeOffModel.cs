namespace TimeKeepingAndPayroll.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TimeOffModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TimeOffs", "Reason", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TimeOffs", "Reason", c => c.String());
        }
    }
}
