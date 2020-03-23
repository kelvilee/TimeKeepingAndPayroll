using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeKeepingAndPayroll.Models
{
    public class FullAddress
    {
        public int ID { get; set; }

        [DisplayName("Room No")]
        public string RoomNo { get; set; }
        public string POBox { get; set; }
        public string Unit { get; set; }
        public string Floor { get; set; }
        public string Wing { get; set; }
        public string Building { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }

        [DisplayName("Postal Code")]
        public string PostalCode { get; set; }
        public string Cell { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }

        public virtual Branch Branch { get; set; }
    }
}