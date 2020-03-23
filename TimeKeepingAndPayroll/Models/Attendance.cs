using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeKeepingAndPayroll.Models
{
    public class Attendance
    {
        public int ID { get; set; }
        public Guid EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }
        public DateTime Timestamp { get; set; }
        public string Activity { get; set; }

    }