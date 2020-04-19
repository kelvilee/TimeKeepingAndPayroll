using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TimeKeepingAndPayroll.Models;

namespace TimeKeepingAndPayroll
{
    public class AppContext : DbContext
    {
        public AppContext() : base("TestTimeKeepingDB2")
        {
            Database.SetInitializer<AppContext>(new AppDBInitializer());
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<Attendance> Attendance { get; set; }
        public DbSet<Branch> Branch { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<FullAddress> FullAddress { get; set; }
        public DbSet<FullName> FullName { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<Service> Service { get; set; }
        public DbSet<Shift> Shift { get; set; }
        public DbSet<TimeOff> TimeOff { get; set; }
    }
    public class AppDBInitializer : CreateDatabaseIfNotExists<AppContext>
    {
        protected override void Seed(AppContext context)
        {
            base.Seed(context);
        }
    }
}