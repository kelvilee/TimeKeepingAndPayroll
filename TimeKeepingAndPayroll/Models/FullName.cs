using System;

namespace TimeKeepingAndPayroll.Models
{
    public class FullName
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string MaidenName { get; set; }
        public virtual Person Person { get; set; }
    }
}