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
                        ID = c.Int(nullable: false, identity: true),
                        EmployeeID = c.Guid(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        Activity = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Employees", t => t.EmployeeID)
                .Index(t => t.EmployeeID);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        BranchID = c.Guid(),
                        HomeAddress_ID = c.Int(),
                        Picture_ID = c.Guid(),
                        WorkAddress_ID = c.Int(),
                        Customer_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Branches", t => t.BranchID)
                .ForeignKey("dbo.FullAddresses", t => t.HomeAddress_ID)
                .ForeignKey("dbo.FullNames", t => t.ID)
                .ForeignKey("dbo.Files", t => t.Picture_ID)
                .ForeignKey("dbo.FullAddresses", t => t.WorkAddress_ID)
                .ForeignKey("dbo.Customers", t => t.Customer_ID)
                .Index(t => t.ID)
                .Index(t => t.BranchID)
                .Index(t => t.HomeAddress_ID)
                .Index(t => t.Picture_ID)
                .Index(t => t.WorkAddress_ID)
                .Index(t => t.Customer_ID);
            
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
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Branches", t => t.Branch_ID)
                .Index(t => t.Branch_ID);
            
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
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        Type = c.String(),
                        Name = c.String(),
                        Content = c.Binary(),
                        Branch_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Branches", t => t.Branch_ID)
                .Index(t => t.Branch_ID);
            
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
                "dbo.ShiftEmployees",
                c => new
                    {
                        Shift_ID = c.Guid(nullable: false),
                        Employee_ID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Shift_ID, t.Employee_ID })
                .ForeignKey("dbo.Shifts", t => t.Shift_ID, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.Employee_ID, cascadeDelete: true)
                .Index(t => t.Shift_ID)
                .Index(t => t.Employee_ID);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        RelationPrimary = c.String(),
                        RelationSecondary = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.People", t => t.ID)
                .Index(t => t.ID);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.People", t => t.ID)
                .Index(t => t.ID);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        EmergencyContactID = c.Guid(),
                        ReportRecipientID = c.Guid(),
                        Role = c.String(),
                        JobTitle = c.String(),
                        EmploymentStatus = c.String(),
                        ReportsTo = c.String(),
                        Groups = c.String(),
                        Description = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.People", t => t.ID)
                .ForeignKey("dbo.Contacts", t => t.EmergencyContactID)
                .ForeignKey("dbo.Employees", t => t.ReportRecipientID)
                .Index(t => t.ID)
                .Index(t => t.EmergencyContactID)
                .Index(t => t.ReportRecipientID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "ReportRecipientID", "dbo.Employees");
            DropForeignKey("dbo.Employees", "EmergencyContactID", "dbo.Contacts");
            DropForeignKey("dbo.Employees", "ID", "dbo.People");
            DropForeignKey("dbo.Customers", "ID", "dbo.People");
            DropForeignKey("dbo.Contacts", "ID", "dbo.People");
            DropForeignKey("dbo.Services", "BranchID", "dbo.Branches");
            DropForeignKey("dbo.ShiftEmployees", "Employee_ID", "dbo.Employees");
            DropForeignKey("dbo.ShiftEmployees", "Shift_ID", "dbo.Shifts");
            DropForeignKey("dbo.Shifts", "Branch_ID", "dbo.Branches");
            DropForeignKey("dbo.People", "Customer_ID", "dbo.Customers");
            DropForeignKey("dbo.People", "WorkAddress_ID", "dbo.FullAddresses");
            DropForeignKey("dbo.People", "Picture_ID", "dbo.Files");
            DropForeignKey("dbo.Files", "Branch_ID", "dbo.Branches");
            DropForeignKey("dbo.People", "ID", "dbo.FullNames");
            DropForeignKey("dbo.People", "HomeAddress_ID", "dbo.FullAddresses");
            DropForeignKey("dbo.FullAddresses", "Branch_ID", "dbo.Branches");
            DropForeignKey("dbo.People", "BranchID", "dbo.Branches");
            DropForeignKey("dbo.Branches", "ParentBranch_ID", "dbo.Branches");
            DropForeignKey("dbo.Attendances", "EmployeeID", "dbo.Employees");
            DropIndex("dbo.Employees", new[] { "ReportRecipientID" });
            DropIndex("dbo.Employees", new[] { "EmergencyContactID" });
            DropIndex("dbo.Employees", new[] { "ID" });
            DropIndex("dbo.Customers", new[] { "ID" });
            DropIndex("dbo.Contacts", new[] { "ID" });
            DropIndex("dbo.ShiftEmployees", new[] { "Employee_ID" });
            DropIndex("dbo.ShiftEmployees", new[] { "Shift_ID" });
            DropIndex("dbo.Services", new[] { "BranchID" });
            DropIndex("dbo.Shifts", new[] { "Branch_ID" });
            DropIndex("dbo.Files", new[] { "Branch_ID" });
            DropIndex("dbo.FullAddresses", new[] { "Branch_ID" });
            DropIndex("dbo.Branches", new[] { "ParentBranch_ID" });
            DropIndex("dbo.Branches", new[] { "Name" });
            DropIndex("dbo.People", new[] { "Customer_ID" });
            DropIndex("dbo.People", new[] { "WorkAddress_ID" });
            DropIndex("dbo.People", new[] { "Picture_ID" });
            DropIndex("dbo.People", new[] { "HomeAddress_ID" });
            DropIndex("dbo.People", new[] { "BranchID" });
            DropIndex("dbo.People", new[] { "ID" });
            DropIndex("dbo.Attendances", new[] { "EmployeeID" });
            DropTable("dbo.Employees");
            DropTable("dbo.Customers");
            DropTable("dbo.Contacts");
            DropTable("dbo.ShiftEmployees");
            DropTable("dbo.Services");
            DropTable("dbo.Shifts");
            DropTable("dbo.Files");
            DropTable("dbo.FullNames");
            DropTable("dbo.FullAddresses");
            DropTable("dbo.Branches");
            DropTable("dbo.People");
            DropTable("dbo.Attendances");
        }
    }
}
