using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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

        // GET: Invoices/Create
        public ActionResult Create()
        {
            ViewBag.Employee_ID = new SelectList(db.Person.OfType<Employee>(), "ID", "Role");
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

            ViewBag.Employee_ID = new SelectList(db.Employee, "ID", "Role", invoice.Employee_ID);
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
    }
}
