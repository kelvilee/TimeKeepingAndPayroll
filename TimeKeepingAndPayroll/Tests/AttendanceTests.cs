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
        }

        [Test]
        public void createAttendance()
        {
            driver.Url = "http://localhost:51729/Attendances/Create";
            IWebElement eid = driver.FindElement(By.XPath(".//*[@id='EmployeeID']"));
            IWebElement time = driver.FindElement(By.XPath(".//*[@id='Timestamp']"));
            IWebElement act = driver.FindElement(By.XPath(".//*[@id='Activity']"));
            var eidSelect = new SelectElement(eid);
            eidSelect.SelectByIndex(1);
            time.SendKeys("04102020");
            time.SendKeys("0545PM");
            var ActivitySelect = new SelectElement(act);
            ActivitySelect.SelectByIndex(2);
            IWebElement createBtn = driver.FindElement(By.XPath(".//*[@class='btn btn-default']"));
            createBtn.Click();
        }

        [Test]
        public void readAttendance()
        {
            driver.Url = "http://localhost:51729/Attendances";
            IWebElement detailsBtn = driver.FindElement(By.XPath("//a[contains(text(),'Details')]"));
            detailsBtn.Click();
            IWebElement backToListBtn = driver.FindElement(By.XPath("//a[contains(text(),'Back to List')]"));
            backToListBtn.Click();
        }

        [Test]
        public void updateAttendance()
        {
            driver.Url = "http://localhost:51729/Attendances";
            IWebElement detailsBtn = driver.FindElement(By.XPath("//a[contains(text(),'Edit')]"));
            detailsBtn.Click();
            IWebElement eid = driver.FindElement(By.XPath(".//*[@id='EmployeeID']"));
            IWebElement time = driver.FindElement(By.XPath(".//*[@id='Timestamp']"));
            IWebElement act = driver.FindElement(By.XPath(".//*[@id='Activity']"));
            var eidSelect = new SelectElement(eid);
            eidSelect.SelectByIndex(1);
            time.SendKeys("05102020");
            time.SendKeys("0745PM");
            var ActivitySelect = new SelectElement(act);
            ActivitySelect.SelectByIndex(1);
            IWebElement createBtn = driver.FindElement(By.XPath(".//*[@class='btn btn-default']"));
            createBtn.Click();
        }

        [Test]
        public void deleteAttendance()
        {
            driver.Url = "http://localhost:51729/Attendances";
            IWebElement deleteBtn = driver.FindElement(By.XPath("//a[contains(text(),'Delete')]"));
            deleteBtn.Click();
            driver.FindElement(By.XPath(".//input[@value='Delete']")).Click();
        }

        [TearDown]
        public void closeBrowser()
        {
            driver.Close();
        }

    }
}