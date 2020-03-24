using System;

namespace TimeKeepingAndPayroll.Models
{
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