using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        public int EmployeeID { get; set; }
        public string Role { get; set; } //[management | staff]
        public string JobTitle { get; set; }
        public string EmploymentStatus { get; set; }
        public string ReportsTo { get; set; } //ReportsTo [drop down list of JobTitles]
        public string Groups { get; set; } //multiselect list
        public string Description { get; set; }
        [StringLength(12, MinimumLength = 8)]
        public string Password { get; set; }
        public int VacationDays { get; set; }
        [Range(0, 999999)]
        public double PayRate { get; set; }
        public DateTime startDate { get; set; }
        public bool canManageAttendance { get; set; }
        public bool canManageTimeOff { get; set; }
        public bool canManagePayroll { get; set; }

        public virtual Contact EmergencyContact { get; set; }
        public virtual Employee ReportRecipient { get; set; }

        public virtual ICollection<Shift> Shifts { get; set; }
        public virtual ICollection<Attendance> AttendanceHistory { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}