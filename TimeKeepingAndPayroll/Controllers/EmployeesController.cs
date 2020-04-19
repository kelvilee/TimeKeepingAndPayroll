using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeKeepingAndPayroll;
using TimeKeepingAndPayroll.Models;

namespace TimeKeepingAndPayroll.Controllers
{
    public class EmployeesController : Controller
    {
        private AppContext db = new AppContext();

        // GET: Employees
        public ActionResult Index()
        {
            var employee = db.Employee.Include(e => e.Branch).Include(e => e.Name).Include(e => e.EmergencyContact).Include(e => e.ReportRecipient);
            var customersPerCity = db.Person.OfType<Employee>().Include("HomeAddress").GroupBy(x => x.HomeAddress.Street).Select(x => new { Street = x.Key, Customers = x.Count() }).ToList();
            Console.WriteLine(customersPerCity);
            return View(employee.ToList());
        }

        // GET: Employees/ViewEmployees/id
        public ActionResult ViewEmployees(Guid? id)
        {
            var employee = db.Employee.Include(e => e.Name).Where(e => e.ReportRecipientID == id);
            return View(employee.ToList());
        }

        // GET: Employees/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Employees/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Employee e)
        {
            var obj = db.Employee.Where(a => a.EmployeeID.Equals(e.EmployeeID) && a.Password.Equals(e.Password)).FirstOrDefault();
            if (obj != null)
            {
                var lastAct = db.Attendance.Where(a => a.EmployeeID.Equals(e.EmployeeID)).OrderByDescending(a => a.Timestamp).FirstOrDefault();
                var status = Status.OUT;
                if (lastAct == null || lastAct.Activity.Equals(Status.OUT))
                {
                    status = Status.IN;
                }
                var attendance = new Attendance
                {
                    Employee = obj,
                    ID = Guid.NewGuid(),
                    EmployeeID = e.EmployeeID,
                    Timestamp = DateTime.Now,
                    Activity = status
                };
                db.Attendance.Add(attendance);
                db.SaveChanges();
                ViewBag.Message = status.Equals(Status.IN) ? "Punched IN" : "Punched OUT";
                return View();
            }
            ViewBag.Message = "Incorrect EmployeeID/Password";
            return View();
        }

        // GET: Employees/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = (Employee)db.Person.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            ViewBag.BranchID = new SelectList(db.Branch, "ID", "Name");
            ViewBag.ID = new SelectList(db.FullName, "ID", "Title");
            ViewBag.EmergencyContactID = new SelectList(db.Employee, "ID", "RelationPrimary");
            ViewBag.ReportRecipientID = new SelectList(db.Employee, "ID", "Role");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,FirstName,MiddleName,LastName,NickName,MaidenName,Person")] FullName name,
            [Bind(Include = "ID,RoomNo,POBox,Unit,Floor,Wing,Building,Street,City,Province,Country,PostalCode,Cell,Phone,Fax,Email")] FullAddress homeAddress,
            [Bind(Include = "ID,RoomNo,POBox,Unit,Floor,Wing,Building,Street,City,Province,Country,PostalCode,Cell,Phone,Fax,Email")] FullAddress workAddress,
            [Bind(Include = "ID,EmployeeID,BranchID,EmergencyContactID,ReportRecipientID,Role,JobTitle,EmploymentStatus,ReportsTo,Groups,Description,Password,PayRate,CanManageAttendance,CanManageTimeOff,CanManagePayroll")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.ID = Guid.NewGuid();
                employee.Name = name;
                employee.HomeAddress = homeAddress;
                employee.WorkAddress = workAddress;
                employee.VacationDays = 15;
                db.Person.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BranchID = new SelectList(db.Branch, "ID", "Name", employee.BranchID);
            ViewBag.ID = new SelectList(db.FullName, "ID", "Title", employee.ID);
            ViewBag.EmergencyContactID = new SelectList(db.Employee, "ID", "RelationPrimary", employee.EmergencyContactID);
            ViewBag.ReportRecipientID = new SelectList(db.Employee, "ID", "Role", employee.ReportRecipientID);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = (Employee)db.Person.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.BranchID = new SelectList(db.Branch, "ID", "Name", employee.BranchID);
            ViewBag.ID = new SelectList(db.FullName, "ID", "Title", employee.ID);
            ViewBag.EmergencyContactID = new SelectList(db.Employee, "ID", "RelationPrimary", employee.EmergencyContactID);
            ViewBag.ReportRecipientID = new SelectList(db.Employee, "ID", "Role", employee.ReportRecipientID);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,BranchID,EmergencyContactID,ReportRecipientID,Role,JobTitle,EmploymentStatus,ReportsTo,Groups,Description,Password,PayRate")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BranchID = new SelectList(db.Branch, "ID", "Name", employee.BranchID);
            ViewBag.ID = new SelectList(db.FullName, "ID", "Title", employee.ID);
            ViewBag.EmergencyContactID = new SelectList(db.Employee, "ID", "RelationPrimary", employee.EmergencyContactID);
            ViewBag.ReportRecipientID = new SelectList(db.Employee, "ID", "Role", employee.ReportRecipientID);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = (Employee)db.Person.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Employee employee = (Employee)db.Person.Find(id);
            db.Person.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
