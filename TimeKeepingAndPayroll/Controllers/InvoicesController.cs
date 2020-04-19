using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using TimeKeepingAndPayroll;
using TimeKeepingAndPayroll.Models;

namespace TimeKeepingAndPayroll.Controllers
{
    public class InvoicesController : Controller
    {
        private AppContext db = new AppContext();

        // GET: Invoices
        public ActionResult EmployeeIndex()
        {
            return View(db.Invoice.ToList());
        }

        public ActionResult ManagerIndex()
        {
            return View(db.Invoice.Include(e => e.Employee).Include(e => e.Employee.Name).ToList());
        }
        // GET: Invoices/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoice.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        public ActionResult CreateSelectDates()
        {
            ViewBag.Employee_ID = new SelectList(db.Person.OfType<Employee>(), "ID", "EmployeeID");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSelectDates([Bind(Include = "Employee_ID, PayPeriodStart, PayPeriodEnd")] Invoice invoice)
        {
            return RedirectToAction("Create", new
            {
                invoice.Employee_ID,
                invoice.PayPeriodStart,
                invoice.PayPeriodEnd,
            });
        }

        public ActionResult Create(Guid Employee_ID, DateTime PayPeriodStart, DateTime PayPeriodEnd)
        {
            ViewBag.Employee_ID = Employee_ID;
            ViewBag.PayPeriodStart = PayPeriodStart;
            ViewBag.PayPeriodEnd = PayPeriodEnd;
            ViewBag.PayDate = DateTime.Now;
            ViewBag.HoursWorked = calculateHoursWorked(Employee_ID, PayPeriodStart, PayPeriodEnd);
            ViewBag.TotalAmount = db.Person.OfType<Employee>().Where(e => e.ID == Employee_ID).FirstOrDefault().PayRate * ViewBag.HoursWorked;
            return View();
        }


        // POST: Invoices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Employee_ID,HoursWorked,PayPeriodStart,PayPeriodEnd,PayDate,TotalAmount,IncomeTax,CPP,IE,Vacation,NetAmount")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                invoice.ID = Guid.NewGuid();
                db.Invoice.Add(invoice);
                db.SaveChanges();
                return RedirectToAction("ManagerIndex");
            }

            ViewBag.Employee_ID = new SelectList(db.Employee, "ID", "EmployeeID", invoice.Employee_ID);
            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoice.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            ViewBag.Employee_ID = new SelectList(db.Person, "ID", "Role", invoice.Employee_ID);
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Employee_ID,HoursWorked,PayPeriodStart,PayPeriodEnd,PayDate,TotalAmount,IncomeTax,CPP,IE,Vacation,NetAmount")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invoice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Employee_ID = new SelectList(db.Person, "ID", "Role", invoice.Employee_ID);
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoice.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Invoice invoice = db.Invoice.Find(id);
            db.Invoice.Remove(invoice);
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

        public void Email(Guid id)
        {
            GMailer.GmailUsername = "batmanatbcit@gmail.com";
            GMailer.GmailPassword = "brucewayne123";

            GMailer mailer = new GMailer();
            mailer.ToEmail = db.Person.OfType<Employee>().Include(e => e.HomeAddress).Where(e => e.ID == id).FirstOrDefault().HomeAddress.Email;
            mailer.Subject = "Your Pay Stuf";
            mailer.Body = "";
            mailer.IsHtml = true;
            mailer.Send();
        }


        // GET: Attendances
        public double calculateHoursWorked(Guid id, DateTime start, DateTime end)
        {
            var attendances = db.Attendance.Include(e => e.Employee).Where(e => e.Employee.ID == id);
                //.Where(e => DbFunctions.TruncateTime(e.Timestamp) >= DbFunctions.TruncateTime(start)).
                //Where(e => DbFunctions.TruncateTime(e.Timestamp) <= DbFunctions.TruncateTime(end));
            TimeSpan output = new TimeSpan(0, 0, 0);
            using (var enumerator = attendances.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var begin = enumerator.Current.Timestamp;
                    if (!enumerator.MoveNext())
                        break;

                    var stop = enumerator.Current.Timestamp;
                    output += (begin - stop);
                }
            }
            return output.TotalHours;
        }
    }
}
