using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    [TestClass]
    public class AttendanceControllerTest
    {
        [TestMethod]
        public void TestIndexView()
        {
            var controller = new AttendancesController();
            var result = controller.Index() as ViewResult;
            Assert.AreEqual("", result.ViewName);

        }

        [TestMethod]
        public void TestInvalidDetailsViewRedirect()
        {
            var controller = new AttendancesController();
            var result = (HttpNotFoundResult)controller.Details(new Guid());
            Assert.AreEqual(404, result.StatusCode);

        }

        [TestMethod]
        public void TestNullDetailsViewRedirect()
        {
            var controller = new AttendancesController();
            var result = (HttpStatusCodeResult)controller.Details(null);
            Assert.AreEqual(400, result.StatusCode);

        }
    }
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
            // populates DropDownList with EmployeeID numbers from Employee
            ViewBag.EmployeeID = new SelectList(db.Employee, "ID", "EmployeeID");
            return View(); // Renders Create view
        }

        // POST: Attendances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Binds the EmployeeID, Timestamp and Activity values from the view to the controller
        public ActionResult Create([Bind(Include = "ID,EmployeeID,Timestamp,Activity")] Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                attendance.ID = Guid.NewGuid(); // Creates a new ID as PK for Attendance table
                db.Attendance.Add(attendance); // Add the attendace to be inserted
                db.SaveChanges(); // Commits the insertion of Attendance record to DB
                return RedirectToAction("Index"); // redirect to Index view of Attendance
            }

            ViewBag.EmployeeID = new SelectList(db.Employee, "ID", "EmployeeID", attendance.EmployeeID); // populates dropdown list with EmployeeID
            return View(attendance); // Render the Create view with the previously entered data populated
        }

        // GET: Attendances/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null) // null id returns HTTP 400
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = db.Attendance.Find(id); // find the Attendance record associated with the id
            if (attendance == null) // return HttpNotFound if attendance is null
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeID = new SelectList(db.Employee, "ID", "EmployeeID", attendance.EmployeeID); // populates dropdown list with EmployeeID
            return View(attendance); // render Edit view with selected Attendance attributes
        }

        // POST: Attendances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Binds the Edit views values to controller
        public ActionResult Edit([Bind(Include = "ID,EmployeeID,Timestamp,Activity")] Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(attendance).State = EntityState.Modified; // Sets the state of the entry as Modified
                var oldTime = db.Entry(attendance).GetDatabaseValues().GetValue<DateTime>("Timestamp").ToString(); // gets the old timestamp
                var oldStatus = db.Entry(attendance).GetDatabaseValues().GetValue<Status>("Activity").ToString(); // gets the old activity
                var newTime = db.Entry(attendance).CurrentValues["Timestamp"]; // gets the new timestamp
                GMailer.GmailUsername = "batmanatbcit@gmail.com"; // gmail sender credentials
                GMailer.GmailPassword = "brucewayne123"; // gmail sender credentials

                GMailer mailer = new GMailer(); // instance of customer mailer class
                mailer.ToEmail = db.Person.OfType<Employee>().Include(e => e.HomeAddress).Where(e => e.ID == attendance.EmployeeID).FirstOrDefault().HomeAddress.Email; // gets the email of employee associated with attendance record
                mailer.Subject = "MVC Health App: Your hours have been changed"; // subject of email
                mailer.Body = $"<h3>Your shift has been changed from:</h3><p>{oldTime} <b>{oldStatus}</b></p>" +
                    $"<h3>to</h3><p>{db.Entry(attendance).CurrentValues["Timestamp"]} <b>{db.Entry(attendance).CurrentValues["Activity"]}</b></p>"; // email contents
                mailer.IsHtml = true;
                db.SaveChanges(); // saves the modified attendance
                mailer.Send(); // send email to employee of modified attendance
                return RedirectToAction("Index"); // redirect to Index view of Attendance
            }
            ViewBag.EmployeeID = new SelectList(db.Employee, "ID", "EmployeeID", attendance.EmployeeID); // populates dropdown list with EmployeeI
            return View(attendance); // render Edit view with selected Attendance attributes
        }

        // GET: Attendances/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null) // null id returns HTTP 400
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = db.Attendance.Find(id); // find the Attendance record associated with the id
            if (attendance == null) // return HttpNotFound if attendance is null
            {
                return HttpNotFound();
            }
            return View(attendance); // render Delete view with selected Attendance attributes
        }

        // POST: Attendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid? id)
        {
            Attendance attendance = db.Attendance.Find(id); // Find the attendance record associated with the ID passed in
            db.Attendance.Remove(attendance); // remove the attendance record
            db.SaveChanges(); // commit the delete of the attendance row
            return RedirectToAction("Index"); // redirect to Index of Attendance
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
