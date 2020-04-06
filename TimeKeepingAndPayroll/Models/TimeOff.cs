using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeKeepingAndPayroll.Models
{
    public class TimeOff
    {
        public int ID { get; set; }
        public Guid EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate{ get; set; }
        public string Reason { get; set; }
        public bool Approved { get; set; }
    }
}