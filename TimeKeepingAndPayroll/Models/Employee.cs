using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TimeKeepingAndPayroll.Models
{
    [Table("Employee")]

    public class Employee : Person
    {
        public Employee()
        {
            Shifts = new HashSet<Shift>();
            AttendanceHistory = new HashSet<Attendance>();
        }

        public Guid? EmergencyContactID { get; set; }
        public Guid? ReportRecipientID { get; set; }

        public int EmployeeID { get; set; }
        public string Role { get; set; } //[management | staff]
        public string JobTitle { get; set; }
        public string EmploymentStatus { get; set; }
        public string ReportsTo { get; set; } //ReportsTo [drop down list of JobTitles]
        public string Groups { get; set; } //multiselect list
        public string Description { get; set; }
        public string Password { get; set; }
        public double PayRate { get; set; }

        public virtual Contact EmergencyContact { get; set; }
        public virtual Employee ReportRecipient { get; set; } //ReportingTo [FK] EmployeeEmergency [Drop Down List of Current Employees]

        public virtual ICollection<Shift> Shifts { get; set; }
        public virtual ICollection<Attendance> AttendanceHistory { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}