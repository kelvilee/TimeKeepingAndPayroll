using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeKeepingAndPayroll.Tests
{
    //check if table that dispays all my invoices is there
    class ViewMyInvoicesTest
    {
        IWebDriver m_driver;

        [Test]
        public void cssDemo()
        {
            m_driver = new ChromeDriver();
            m_driver.Url = "http://localhost:51729/Invoices/EmployeeIndex";
            m_driver.Manage().Window.Maximize();
            IWebElement table = m_driver.FindElement(By.XPath(".//table[@class='table']"));
            m_driver.Close();
        }
    }
}