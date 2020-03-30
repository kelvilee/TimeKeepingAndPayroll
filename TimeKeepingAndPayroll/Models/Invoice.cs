using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TimeKeepingAndPayroll.Models
{
    public class Invoice
    {
        public Guid ID { get; set; }

        public virtual Employee Employee { get; set; }

        public double HoursWorked { get; set; }

        public DateTime PayPeriodStart { get; set; }

        public DateTime PayPeriodEnd { get; set; }

        public DateTime PayDate { get; set; }

        public double TotalAmount { get; set; }

        public double IncomeTax { get; set; }

        public double CPP { get; set; }

        public double IE { get; set; }

        public double Vacation { get; set; }

        public double NetAmount { get; set; }

    }
}