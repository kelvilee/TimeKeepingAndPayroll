using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Globalization;

namespace TimeKeepingAndPayroll.Tests
{
    class AttendanceTests
    {
        IWebDriver driver;

        [SetUp]
        public void startBrowser()
        {
            driver = new ChromeDriver();
            driver.Url = "http://localhost:51729/"; // you will need to run IIS Express on another instance of VS and run these tests on this one, make sure this matches your localhost:PORT# of the running instance of IIS Express
        }

        [Test]
        public void managerFixTime()
        {
            IWebElement employeeNav = driver.FindElement(By.XPath("//a[@href='/Employees']"));
            employeeNav.Click();
            IWebElement getEmployees = driver.FindElement(By.XPath(".//*[@class='table']//tbody/tr[2]/td[11]/a[4]"));
            getEmployees.Click();
            IWebElement editAttendance = driver.FindElement(By.XPath(".//*[@class='table']//tbody/tr[2]/td[7]/a[1]"));
            editAttendance.Click();
            IWebElement createAttendance = driver.FindElement(By.XPath("//a[@href='/Attendances/Create']"));
            createAttendance.Click();
            IWebElement eid = driver.FindElement(By.XPath(".//*[@id='EmployeeID']"));
            IWebElement time = driver.FindElement(By.XPath(".//*[@id='Timestamp']"));
            IWebElement act = driver.FindElement(By.XPath(".//*[@id='Activity']"));
            var eidSelect = new SelectElement(eid);
            eidSelect.SelectByValue("2222");
            time.SendKeys("04102020");
            time.SendKeys("0545PM");
            var ActivitySelect = new SelectElement(act);
            ActivitySelect.SelectByIndex(2);
            IWebElement createBtn = driver.FindElement(By.XPath(".//*[@class='btn btn-default']"));
            createBtn.Click();
        }

        [Test]
        public void employeePunchIn()
        {
            IWebElement loginNav = driver.FindElement(By.XPath("//a[@href='/Employees/Login']"));
            loginNav.Click();
            IWebElement eid = driver.FindElement(By.XPath(".//*[@id='EmployeeID']"));
            IWebElement pass = driver.FindElement(By.XPath(".//*[@id='Password']"));
            eid.SendKeys("2222");
            pass.SendKeys("123");
            IWebElement loginBtn = driver.FindElement(By.XPath(".//*[@class='btn btn-default']"));
            loginBtn.Click();
        }

        [TearDown]
        public void closeBrowser()
        {
            driver.Close();
        }

    }
}