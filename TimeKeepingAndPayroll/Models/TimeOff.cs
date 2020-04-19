using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TimeKeepingAndPayroll.Models
{
    public class TimeOff
    {
        public int ID { get; set; }
        [ForeignKey("Employee")]
        public Guid EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }
        [ForeignKey("Replacement")]
        public Guid? ReplacementID { get; set; }
        public virtual Employee Replacement { get; set; }
        [ValidStartDate]
        public DateTime StartDate { get; set; }
        [ValidEndDate("StartDate")]
        public DateTime EndDate { get; set; }
        [StringLength(50)]
        public string Reason { get; set; }
        public bool Approved { get; set; }
    }

    public class ValidStartDate : ValidationAttribute
    {
        public override bool IsValid(object date)
        {
            DateTime sdate = (DateTime)date;
            return sdate >= DateTime.Today;
        }
    }

    public class ValidEndDate : ValidationAttribute
    {
        private readonly string sdate;

        public ValidEndDate(string sdate)
        {
            this.sdate = sdate;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(this.sdate);
            var StartDateValue = property.GetValue(validationContext.ObjectInstance, null);
            DateTime StartDate = DateTime.MinValue;
            DateTime EndDate = DateTime.MinValue;

            if (DateTime.TryParse(StartDateValue.ToString(), out StartDate))
            {
                if (DateTime.TryParse(value.ToString(), out EndDate))
                {
                    if (EndDate >= StartDate)
                    {
                        return ValidationResult.Success;
                    }
                    else
                    {
                        return new ValidationResult(FormatErrorMessage("End Date"));
                    }
                }
                else
                {
                    return new ValidationResult(FormatErrorMessage("End Date"));
                }
            }
            else
            {
                return new ValidationResult(FormatErrorMessage("Start Date"));
            }
        }
    }
}