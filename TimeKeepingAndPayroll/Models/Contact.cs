using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TimeKeepingAndPayroll.Models
{
    [Table("Contact")]

    public class Contact : Person
    {
        public string RelationPrimary { get; set; } //Dropdown List
        public string RelationSecondary { get; set; }
        public string Description { get; set; } //Key Value Pairs

    }
}