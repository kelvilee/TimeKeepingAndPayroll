namespace TimeKeepingAndPayroll.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class employeeStartDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employee", "startDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employee", "startDate");
        }
    }
}
