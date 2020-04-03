using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TimeKeepingAndPayroll.Models
{
    public class Attendance
    {
        public Guid ID { get; set; }
        public DateTime Timestamp { get; set; }
        public Status Activity { get; set; }

        public int EmployeeID { get; set; }
        public Employee Employee { get; set; }

    }

    public enum Status
    {
        IN,
        OUT
    }
}