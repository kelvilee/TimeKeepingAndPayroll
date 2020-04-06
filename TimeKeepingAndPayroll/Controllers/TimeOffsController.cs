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
    public class TimeOffsController : Controller
    {
        private AppContext db = new AppContext();

        // GET: TimeOffs
        public ActionResult Index()
        {
            var timesOff = db.TimeOff.Include(t => t.Employee);
            return View(timesOff.ToList());
        }

        // GET: TimeOffs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeOff timeOff = db.TimeOff.Find(id);
            if (timeOff == null)
            {
                return HttpNotFound();
            }
            return View(timeOff);
        }

        // GET: TimeOffs/Create
        public ActionResult Create()
        {
            ViewBag.EmployeeID = new SelectList(db.Employee.Include("Name"), "ID", "Name.FirstName");
            return View();
        }

        // POST: TimeOffs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,EmployeeID,StartDate,EndDate,Reason")] TimeOff timeOff)
        {
            if (ModelState.IsValid)
            {
                timeOff.Approved = false;
                db.TimeOff.Add(timeOff);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeID = new SelectList(db.Employee.Include("Name"), "ID", "Name.FirstName");
            return View(timeOff);
        }

        // GET: TimeOffs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeOff timeOff = db.TimeOff.Find(id);
            if (timeOff == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeID = new SelectList(db.Employee.Include("Name"), "ID", "Name.FirstName");
            return View(timeOff);
        }

        // POST: TimeOffs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,EmployeeID,StartDate,EndDate,Reason")] TimeOff timeOff)
        {
            if (ModelState.IsValid)
            {
                db.Entry(timeOff).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeID = new SelectList(db.Employee.Include("Name"), "ID", "Name.FirstName");
            return View(timeOff);
        }

        // GET: TimeOffs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeOff timeOff = db.TimeOff.Find(id);
            if (timeOff == null)
            {
                return HttpNotFound();
            }
            return View(timeOff);
        }

        // POST: TimeOffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TimeOff timeOff = db.TimeOff.Find(id);
            db.TimeOff.Remove(timeOff);
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
