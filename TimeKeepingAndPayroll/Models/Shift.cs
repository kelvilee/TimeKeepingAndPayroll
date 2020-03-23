using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeKeepingAndPayroll.Models
{
    public class Shift : Event
    {
        public Shift()
        {
            Employees = new HashSet<Employee>();

        }

        public string Title { get; set; }
        public string Description { get; set; }

        public virtual Branch Branch { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }

    }
}