using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeKeepingAndPayroll.Models
{
    [Table("People")]

    public class Person
    {

        public Guid ID { get; set; }

        public Guid? BranchID { get; set; }

        public virtual Branch Branch { get; set; }
        public virtual FullName Name { get; set; }
        public virtual FullAddress HomeAddress { get; set; }
        public virtual FullAddress WorkAddress { get; set; }
        //public virtual File Picture { get; set; }
    }
}