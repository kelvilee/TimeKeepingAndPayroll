using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeKeepingAndPayroll.Models
{
    public class FullName
    {
        public Guid ID { get; set; }

        public string Title { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Middle Name")]
        public string MiddleName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        public string NickName { get; set; }
        public string MaidenName { get; set; }

        public virtual Person Person { get; set; }
    }
}