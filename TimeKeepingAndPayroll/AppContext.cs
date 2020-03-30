using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using TimeKeepingAndPayroll.Models;

namespace TimeKeepingAndPayroll
{
    public class AppContext : DbContext
    {
        public AppContext() : base("TimeKeepingDB")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<FullAddress> FullAddresses { get; set; }
        public DbSet<FullName> FullNames { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Person> People { get; set; }
    }

    public class AppDBInitializer : CreateDatabaseIfNotExists<AppContext>
    {
        protected override void Seed(AppContext context)
        {
            base.Seed(context);
        }
    }
}