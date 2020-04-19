using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeKeepingAndPayroll.Tests
{
    //check if table that dispays all my invoices is there
    class TimeOffTests
    {
        IWebDriver m_driver;

        [Test]
        public void approveTimeOff()
        {
            m_driver = new ChromeDriver();
            m_driver.Url = "http://localhost:51729/TimeOffs/ManagerIndex";
            m_driver.Manage().Window.Maximize();
            IWebElement approveRequestBtn = m_driver.FindElement(By.XPath(".//table//td//a[contains(text(),'Approve')]"));
            approveRequestBtn.Click();
            var employeeddl = m_driver.FindElement(By.Id("ReplacementID"));
            var selectElement = new SelectElement(employeeddl);
            selectElement.SelectByText("John");
            m_driver.FindElement(By.XPath(".//input[@value='Save']")).Submit();

            Assert.IsTrue(m_driver.FindElement(By.XPath(".//table//td//input[@type='checkbox']")).Selected);
            m_driver.FindElement(By.XPath(".//table//td[contains(text(),'John Smith')]"));
            m_driver.Close();
        }

        [Test]
        public void requestTimeOff()
        {
            m_driver = new ChromeDriver();
            m_driver.Url = "http://localhost:51729/TimeOffs/Index";
            m_driver.Manage().Window.Maximize();
            IWebElement approveRequestBtn = m_driver.FindElement(By.XPath(".//a[contains(text(),'Create New')]"));
            approveRequestBtn.Click();
            var employeeddl = m_driver.FindElement(By.Id("EmployeeID"));
            var selectElement = new SelectElement(employeeddl);
            selectElement.SelectByText("Jane");
            m_driver.FindElement(By.Id("StartDate")).SendKeys("04/01/2020");
            m_driver.FindElement(By.Id("EndDate")).SendKeys("04/10/2020");
            m_driver.FindElement(By.Id("Reason")).SendKeys("Sick");
            m_driver.FindElement(By.XPath(".//input[@value='Create']")).Submit();

            m_driver.FindElement(By.XPath(".//table//td[contains(text(), '2020-04-01 12:00:00 AM')]")).Click();
            m_driver.Close();

        }
    }
}