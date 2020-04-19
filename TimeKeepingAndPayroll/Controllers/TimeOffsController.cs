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
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var id = Guid.Parse(Session["ID"].ToString());
            var timesOff = db.TimeOff.Include(t => t.Employee).Include(t => t.Employee.Name).Where(t => t.Employee.ID == id).OrderByDescending(t => t.StartDate);
            var pendingRequests = db.TimeOff.Where(t => t.Approved == false)
                .GroupBy(x => x.Approved)
                .Select(x => new { Approved = x.Key, Requests = x.Count() })
                .FirstOrDefault();
            ViewBag.RemainingVacationDays = db.Employee.Find(Session["ID"]).VacationDays;
            if(pendingRequests == null)
            {
                ViewBag.PendingRequests = 0;
            } else
            {
                ViewBag.PendingRequests = pendingRequests.Requests;
            }
            return View(timesOff.ToList());
        }

        // GET: TimeOffs
        public ActionResult ManagerIndex()
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var id = Guid.Parse(Session["ID"].ToString());
            if (db.Person.OfType<Employee>().Where(t => t.ID == id).FirstOrDefault().canManageTimeOff != true)
            {
                TempData["AlertMessage"] = "You cannot manage times off. No sufficient permissions.";
                return RedirectToAction("Index");
            }
            var timesOff = db.TimeOff.Include(t => t.Employee).Include(t => t.Employee.Name).Include(t => t.Replacement.Name);
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
            if(Session["ID"] == null)
            {
                return RedirectToAction("Index","Home");
            }
            var id = Guid.Parse(Session["ID"].ToString());
            var emp = db.Employee.Include("Name").Where(t => t.ID == id);
            ViewBag.EmployeeID = new SelectList(emp, "ID", "Name.FirstName");
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
                Employee emp = db.Employee.Find(Session["ID"]);
                emp.VacationDays -= (timeOff.EndDate - timeOff.StartDate).Days;
                timeOff.Approved = false;
                db.TimeOff.Add(timeOff);
                db.Entry(emp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeID = new SelectList(db.Employee.Include("Name"), "ID", "Name.FirstName");
            return View(timeOff);
        }

        // GET: TimeOffs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeOff timeOff = db.TimeOff.Find(id);
            if (timeOff == null)
            {
                return HttpNotFound();
            }
            var curId = Guid.Parse(Session["ID"].ToString());
            var emp = db.Employee.Include("Name").Where(t => t.ID == curId);
            ViewBag.EmployeeID = new SelectList(emp, "ID", "Name.FirstName");
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
            Employee emp = db.Employee.Find(Session["ID"]);
            emp.VacationDays += (timeOff.EndDate - timeOff.StartDate).Days;
            db.Entry(emp).State = EntityState.Modified;
            db.TimeOff.Remove(timeOff);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: TimeOffs/Approve/5
        public ActionResult Approve(int? id)
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeOff timeOff = db.TimeOff.Find(id);
            if (timeOff == null)
            {
                return HttpNotFound();
            }
            ViewBag.ReplacementID = new SelectList(db.Employee.Include("Name").Where(t => t.ID != timeOff.EmployeeID).OrderByDescending(t => t.startDate), "ID", "Name.FirstName");
            return View(timeOff);
        }

        // POST: TimeOffs/Approve/5
        [HttpPost, ActionName("Approve")]
        [ValidateAntiForgeryToken]
        public ActionResult ApproveRequest(TimeOff timeOff)
        {
            if (ModelState.IsValid)
            {
                TimeOff newTimeOff = db.TimeOff.Find(timeOff.ID);
                Employee newReplacement = db.Employee.Find(timeOff.ReplacementID);
                newTimeOff.Replacement = newReplacement;
                newTimeOff.Approved = true;
                db.Entry(newTimeOff).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ManagerIndex");
            }

            var curId = Guid.Parse(Session["ID"].ToString());

            ViewBag.ReplacementID = new SelectList(db.Employee.Include("Name").Where(t => t.ID != curId).OrderByDescending(t => t.startDate), "ID", "Name.FirstName");
            return View(timeOff);
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
