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
            m_driver.Url = "http://localhost:51729/TimeOff/EmployeeIndex";
            m_driver.Manage().Window.Maximize();
            IWebElement table = m_driver.FindElement(By.XPath(".//table[@class='table']"));
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
            m_driver.FindElement(By.Id("StartDate")).SendKeys("01/01/2020");
            m_driver.FindElement(By.Id("EndDate")).SendKeys("01/14/2020");
            m_driver.FindElement(By.Id("Reason")).SendKeys("Sick");
            m_driver.FindElement(By.XPath(".//input[@value='Create']")).Click();

            m_driver.FindElement(By.XPath(".//li//a[contains(text(), 'Approve Time Off')]")).Click();

            m_driver.FindElement(By.XPath(".//table//td[contains(text(), '2020-01-01 12:00:00 AM')]")).Click();
            m_driver.Close();

        }
    }
}