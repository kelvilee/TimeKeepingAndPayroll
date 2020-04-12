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
    public class AttendancesController : Controller
    {
        private AppContext db = new AppContext();

        // GET: Attendances
        public ActionResult Index()
        {
            var attendance = db.Attendance.Include(a => a.Employee);
            return View(attendance.ToList());
        }

        // GET: Attendances/id
        public ActionResult EditAttendance(int? id)
        {
            var attendances = db.Attendance.Where(e => e.Employee.EmployeeID == id).OrderByDescending(t => t.Timestamp);
            return View(attendances.ToList());
        }

        // GET: Attendances
        public ActionResult HoursWorked(int? id)
        {
            var attendances = db.Attendance.Where(e => e.Employee.EmployeeID == id).OrderByDescending(t => t.Timestamp);
            TimeSpan output = new TimeSpan(0, 0, 0);
            using (var enumerator = attendances.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var begin = enumerator.Current.Timestamp;
                    if (!enumerator.MoveNext())
                        break;

                    var end = enumerator.Current.Timestamp;
                    output += (begin - end);
                }
            }
            ViewBag.Message = output.TotalHours;
            return View(attendances.ToList());
        }

        // GET: Attendances/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = db.Attendance.Find(id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            return View(attendance);
        }

        // GET: Attendances/Create
        public ActionResult Create()
        {
            ViewBag.EmployeeID = new SelectList(db.Employee, "EmployeeID", "EmployeeID");
            return View();
        }

        // POST: Attendances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,EmployeeID,Timestamp,Activity")] Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                attendance.ID = Guid.NewGuid();
                db.Attendance.Add(attendance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeID = new SelectList(db.Employee, "ID", "EmployeeID", attendance.EmployeeID);
            return View(attendance);
        }

        // GET: Attendances/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = db.Attendance.Find(id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeID = new SelectList(db.Employee, "EmployeeID", "EmployeeID", attendance.EmployeeID);
            return View(attendance);
        }

        // POST: Attendances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,EmployeeID,Timestamp,Activity")] Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(attendance).State = EntityState.Modified;
                var oldTime = db.Entry(attendance).GetDatabaseValues().GetValue<DateTime>("Timestamp").ToString();
                var oldStatus = db.Entry(attendance).GetDatabaseValues().GetValue<Status>("Activity").ToString();
                var newTime = db.Entry(attendance).CurrentValues["Timestamp"];
                GMailer.GmailUsername = "batmanatbcit@gmail.com";
                GMailer.GmailPassword = "brucewayne123";

                GMailer mailer = new GMailer();
                mailer.ToEmail = db.Person.OfType<Employee>().Include(e => e.HomeAddress).Where(e => e.EmployeeID == attendance.Employee.EmployeeID).FirstOrDefault().HomeAddress.Email;
                mailer.Subject = "MVC Health App: Your hours have been changed";
                mailer.Body = $"<h3>Your shift has been changed from:</h3><p>{oldTime} <b>{oldStatus}</b></p>" +
                    $"<h3>to</h3><p>{db.Entry(attendance).CurrentValues["Timestamp"]} <b>{db.Entry(attendance).CurrentValues["Activity"]}</b></p>";
                mailer.IsHtml = true;
                db.SaveChanges();
                mailer.Send();
                return RedirectToAction("EditAttendance", "Attendances", new { id = attendance.EmployeeID });
            }
            ViewBag.EmployeeID = new SelectList(db.Employee, "ID", "EmployeeID", attendance.EmployeeID);
            return View(attendance);
        }

        // GET: Attendances/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = db.Attendance.Find(id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            return View(attendance);
        }

        // POST: Attendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid? id)
        {
            Attendance attendance = db.Attendance.Find(id);
            db.Attendance.Remove(attendance);
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

    public class GMailer
    {
        public static string GmailUsername { get; set; }
        public static string GmailPassword { get; set; }
        public static string GmailHost { get; set; }
        public static int GmailPort { get; set; }
        public static bool GmailSSL { get; set; }

        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }

        static GMailer()
        {
            GmailHost = "smtp.gmail.com";
            GmailPort = 587; // Gmail can use ports 25, 465 & 587; but must be 25 for medium trust environment.
            GmailSSL = true;
        }

        public void Send()
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = GmailHost;
            smtp.Port = GmailPort;
            smtp.EnableSsl = GmailSSL;
            smtp.UseDefaultCredentials = false;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = new NetworkCredential(GmailUsername, GmailPassword);

            using (var message = new MailMessage(GmailUsername, ToEmail))
            {
                message.Subject = Subject;
                message.Body = Body;
                message.IsBodyHtml = IsHtml;
                smtp.Send(message);
            }
        }
    }
}
