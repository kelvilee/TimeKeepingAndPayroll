using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TimeKeepingAndPayroll.Models
{
    [Table("Customer")]
    public class Customer : Person
    {
        public Customer()
        {
            Contacts = new HashSet<Contact>();
        }
        public virtual ICollection<Contact> Contacts { get; set; }
    }
}