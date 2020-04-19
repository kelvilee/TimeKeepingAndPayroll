namespace TimeKeepingAndPayroll.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attendances",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        Activity = c.Int(nullable: false),
                        EmployeeID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Employee", t => t.EmployeeID)
                .Index(t => t.EmployeeID);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        BranchID = c.Guid(),
                        HomeAddress_ID = c.Int(),
                        WorkAddress_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Branches", t => t.BranchID)
                .ForeignKey("dbo.FullAddresses", t => t.HomeAddress_ID)
                .ForeignKey("dbo.FullAddresses", t => t.WorkAddress_ID)
                .Index(t => t.BranchID)
                .Index(t => t.HomeAddress_ID)
                .Index(t => t.WorkAddress_ID);
            
            CreateTable(
                "dbo.Branches",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        ParentID = c.Guid(),
                        Name = c.String(maxLength: 32),
                        Street = c.String(),
                        City = c.String(),
                        Province = c.String(),
                        Country = c.String(),
                        PostalCode = c.String(),
                        Phone = c.String(),
                        Fax = c.String(),
                        Email = c.String(),
                        Website = c.String(),
                        ParentBranch_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Branches", t => t.ParentBranch_ID)
                .Index(t => t.Name, unique: true)
                .Index(t => t.ParentBranch_ID);
            
            CreateTable(
                "dbo.FullAddresses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RoomNo = c.String(),
                        POBox = c.String(),
                        Unit = c.String(),
                        Floor = c.String(),
                        Wing = c.String(),
                        Building = c.String(),
                        Street = c.String(),
                        City = c.String(),
                        Province = c.String(),
                        Country = c.String(),
                        PostalCode = c.String(),
                        Cell = c.String(),
                        Phone = c.String(),
                        Fax = c.String(),
                        Email = c.String(),
                        Branch_ID = c.Guid(),
                        Person_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Branches", t => t.Branch_ID)
                .ForeignKey("dbo.People", t => t.Person_ID)
                .Index(t => t.Branch_ID)
                .Index(t => t.Person_ID);
            
            CreateTable(
                "dbo.FullNames",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Title = c.String(),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        LastName = c.String(),
                        NickName = c.String(),
                        MaidenName = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.People", t => t.ID)
                .Index(t => t.ID);
            
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Employee_ID = c.Guid(nullable: false),
                        HoursWorked = c.Double(nullable: false),
                        PayPeriodStart = c.DateTime(nullable: false),
                        PayPeriodEnd = c.DateTime(nullable: false),
                        PayDate = c.DateTime(nullable: false),
                        TotalAmount = c.Double(nullable: false),
                        IncomeTax = c.Double(nullable: false),
                        CPP = c.Double(nullable: false),
                        IE = c.Double(nullable: false),
                        Vacation = c.Double(nullable: false),
                        NetAmount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Employee", t => t.Employee_ID)
                .Index(t => t.Employee_ID);
            
            CreateTable(
                "dbo.Shifts",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        Branch_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Branches", t => t.Branch_ID)
                .Index(t => t.Branch_ID);
            
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BranchID = c.Guid(nullable: false),
                        Category = c.String(),
                        SubCategory = c.String(),
                        Participants = c.String(),
                        Description = c.String(),
                        CostPerUnit = c.Decimal(precision: 18, scale: 2),
                        MinutePerUnit = c.Int(),
                        Personalized = c.Boolean(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Branches", t => t.BranchID, cascadeDelete: true)
                .Index(t => t.BranchID);
            
            CreateTable(
                "dbo.TimeOffs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        EmployeeID = c.Guid(nullable: false),
                        ReplacementID = c.Guid(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Reason = c.String(),
                        Approved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Employee", t => t.EmployeeID)
                .ForeignKey("dbo.Employee", t => t.ReplacementID)
                .Index(t => t.EmployeeID)
                .Index(t => t.ReplacementID);
            
            CreateTable(
                "dbo.ShiftEmployees",
                c => new
                    {
                        Shift_ID = c.Guid(nullable: false),
                        Employee_ID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Shift_ID, t.Employee_ID })
                .ForeignKey("dbo.Shifts", t => t.Shift_ID, cascadeDelete: true)
                .ForeignKey("dbo.Employee", t => t.Employee_ID, cascadeDelete: true)
                .Index(t => t.Shift_ID)
                .Index(t => t.Employee_ID);
            
            CreateTable(
                "dbo.Contact",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Customer_ID = c.Guid(),
                        RelationPrimary = c.String(),
                        RelationSecondary = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.People", t => t.ID)
                .ForeignKey("dbo.Customer", t => t.Customer_ID)
                .Index(t => t.ID)
                .Index(t => t.Customer_ID);
            
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.People", t => t.ID)
                .Index(t => t.ID);
            
            CreateTable(
                "dbo.Employee",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        EmergencyContactID = c.Guid(),
                        ReportRecipientID = c.Guid(),
                        EmployeeID = c.Int(nullable: false),
                        Role = c.String(),
                        JobTitle = c.String(),
                        EmploymentStatus = c.String(),
                        ReportsTo = c.String(),
                        Groups = c.String(),
                        Description = c.String(),
                        Password = c.String(maxLength: 12),
                        VacationDays = c.Int(nullable: false),
                        PayRate = c.Double(nullable: false),
                        canManageAttendance = c.Boolean(nullable: false),
                        canManageTimeOff = c.Boolean(nullable: false),
                        canManagePayroll = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.People", t => t.ID)
                .ForeignKey("dbo.Contact", t => t.EmergencyContactID)
                .ForeignKey("dbo.Employee", t => t.ReportRecipientID)
                .Index(t => t.ID)
                .Index(t => t.EmergencyContactID)
                .Index(t => t.ReportRecipientID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employee", "ReportRecipientID", "dbo.Employee");
            DropForeignKey("dbo.Employee", "EmergencyContactID", "dbo.Contact");
            DropForeignKey("dbo.Employee", "ID", "dbo.People");
            DropForeignKey("dbo.Customer", "ID", "dbo.People");
            DropForeignKey("dbo.Contact", "Customer_ID", "dbo.Customer");
            DropForeignKey("dbo.Contact", "ID", "dbo.People");
            DropForeignKey("dbo.TimeOffs", "ReplacementID", "dbo.Employee");
            DropForeignKey("dbo.TimeOffs", "EmployeeID", "dbo.Employee");
            DropForeignKey("dbo.Services", "BranchID", "dbo.Branches");
            DropForeignKey("dbo.ShiftEmployees", "Employee_ID", "dbo.Employee");
            DropForeignKey("dbo.ShiftEmployees", "Shift_ID", "dbo.Shifts");
            DropForeignKey("dbo.Shifts", "Branch_ID", "dbo.Branches");
            DropForeignKey("dbo.Invoices", "Employee_ID", "dbo.Employee");
            DropForeignKey("dbo.People", "WorkAddress_ID", "dbo.FullAddresses");
            DropForeignKey("dbo.FullNames", "ID", "dbo.People");
            DropForeignKey("dbo.People", "HomeAddress_ID", "dbo.FullAddresses");
            DropForeignKey("dbo.FullAddresses", "Person_ID", "dbo.People");
            DropForeignKey("dbo.FullAddresses", "Branch_ID", "dbo.Branches");
            DropForeignKey("dbo.People", "BranchID", "dbo.Branches");
            DropForeignKey("dbo.Branches", "ParentBranch_ID", "dbo.Branches");
            DropForeignKey("dbo.Attendances", "EmployeeID", "dbo.Employee");
            DropIndex("dbo.Employee", new[] { "ReportRecipientID" });
            DropIndex("dbo.Employee", new[] { "EmergencyContactID" });
            DropIndex("dbo.Employee", new[] { "ID" });
            DropIndex("dbo.Customer", new[] { "ID" });
            DropIndex("dbo.Contact", new[] { "Customer_ID" });
            DropIndex("dbo.Contact", new[] { "ID" });
            DropIndex("dbo.ShiftEmployees", new[] { "Employee_ID" });
            DropIndex("dbo.ShiftEmployees", new[] { "Shift_ID" });
            DropIndex("dbo.TimeOffs", new[] { "ReplacementID" });
            DropIndex("dbo.TimeOffs", new[] { "EmployeeID" });
            DropIndex("dbo.Services", new[] { "BranchID" });
            DropIndex("dbo.Shifts", new[] { "Branch_ID" });
            DropIndex("dbo.Invoices", new[] { "Employee_ID" });
            DropIndex("dbo.FullNames", new[] { "ID" });
            DropIndex("dbo.FullAddresses", new[] { "Person_ID" });
            DropIndex("dbo.FullAddresses", new[] { "Branch_ID" });
            DropIndex("dbo.Branches", new[] { "ParentBranch_ID" });
            DropIndex("dbo.Branches", new[] { "Name" });
            DropIndex("dbo.People", new[] { "WorkAddress_ID" });
            DropIndex("dbo.People", new[] { "HomeAddress_ID" });
            DropIndex("dbo.People", new[] { "BranchID" });
            DropIndex("dbo.Attendances", new[] { "EmployeeID" });
            DropTable("dbo.Employee");
            DropTable("dbo.Customer");
            DropTable("dbo.Contact");
            DropTable("dbo.ShiftEmployees");
            DropTable("dbo.TimeOffs");
            DropTable("dbo.Services");
            DropTable("dbo.Shifts");
            DropTable("dbo.Invoices");
            DropTable("dbo.FullNames");
            DropTable("dbo.FullAddresses");
            DropTable("dbo.Branches");
            DropTable("dbo.People");
            DropTable("dbo.Attendances");
        }
    }
}
