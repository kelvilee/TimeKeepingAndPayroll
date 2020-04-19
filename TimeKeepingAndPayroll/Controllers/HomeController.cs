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
            if (Session["Name"] != null)
            {
                return RedirectToAction("Index", "Employees");
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session["Manager"] = false;
            Session["Name"] = null;
            Session["ID"] = null;
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
                if(obj.Role == "Manager")
                {
                    Session["Manager"] = true;
                } else
                {
                    Session["Manager"] = false;
                }
                Session["Name"] = obj.EmployeeID;
                Session["ID"] = obj.ID;
                return RedirectToAction("HoursWorked", "Attendances", new { id = obj.EmployeeID });
            }
            // login fail
            Session["Manager"] = false;
            Session["Name"] = null;
            Session["ID"] = null;
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