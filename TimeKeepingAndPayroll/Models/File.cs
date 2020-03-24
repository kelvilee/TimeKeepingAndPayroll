using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeKeepingAndPayroll.Models
{
    public class File
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public byte[] Content { get; set; }
        public virtual Branch Branch { get; set; }
    }
}