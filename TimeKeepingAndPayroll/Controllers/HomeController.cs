using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using TimeKeepingAndPayroll.Models;

namespace TimeKeepingAndPayroll.Controllers
{
    public class HomeController : Controller
    {
        private AppContext db = new AppContext();
        public ActionResult Index()
        {
            if(Session["Name"] != null)
            {
                return RedirectToAction("Index", "Employees");
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session["Name"] = null;
            return View("Index");
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Employee e)
        {
            var obj = db.Employee.Where(a => a.EmployeeID.Equals(e.EmployeeID) && a.Password.Equals(e.Password)).FirstOrDefault();
            if (obj != null)
            {
                // login success
                Session["Name"] = obj.EmployeeID;
                return RedirectToAction("Index", "Employees");
            }
            // login fail
            Session["Name"] = null;
            ViewBag.Message = "Incorrect EmployeeID/Password";
            return View("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}