using System;

namespace TimeKeepingAndPayroll.Models
{
    public class Service
    {
        public int ID { get; set; }
        public Guid BranchID { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Participants { get; set; } //multi-select list of job titles
        public string Description { get; set; }
        public decimal? CostPerUnit { get; set; }
        public int? MinutePerUnit { get; set; }
        public bool? Personalized { get; set; }
        public virtual Branch Branch { get; set; }
    }
}