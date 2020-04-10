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
    class InvoicesTests
    {
        IWebDriver m_driver;

        [Test]
        public void viewEmployeeInvoices()
        {
            m_driver = new ChromeDriver();
            m_driver.Url = "http://localhost:51729/Invoices/EmployeeIndex";
            m_driver.Manage().Window.Maximize();
            IWebElement table = m_driver.FindElement(By.XPath(".//table[@class='table']"));
            m_driver.Close();
        }

        [Test]
        public void createInvoice()
        {
            m_driver = new ChromeDriver();
            m_driver.Url = "http://localhost:51729/Invoices/ManagerIndex";
            m_driver.Manage().Window.Maximize();
            IWebElement createNewInvoiceBtn = m_driver.FindElement(By.XPath(".//a[contains(text(),'Create')]"));
            createNewInvoiceBtn.Click();
            var employeeddl = m_driver.FindElement(By.Id("Employee_ID"));
            var selectElement = new SelectElement(employeeddl);
            selectElement.SelectByText("Manager");
            m_driver.FindElement(By.Id("HoursWorked")).SendKeys("40");
            m_driver.FindElement(By.Id("PayPeriodStart")).SendKeys("01/01/2020");
            m_driver.FindElement(By.Id("PayPeriodEnd")).SendKeys("01/14/2020");
            m_driver.FindElement(By.Id("PayDate")).SendKeys("01/17/2020");
            m_driver.FindElement(By.Id("TotalAmount")).SendKeys("1600");
            m_driver.FindElement(By.Id("IncomeTax")).SendKeys("300");
            m_driver.FindElement(By.Id("CPP")).SendKeys("30");
            m_driver.FindElement(By.Id("IE")).SendKeys("30");
            m_driver.FindElement(By.Id("IE")).SendKeys("30");
            m_driver.FindElement(By.Id("Vacation")).SendKeys("13");
            m_driver.FindElement(By.Id("NetAmount")).SendKeys("1227");
            m_driver.FindElement(By.XPath(".//input[@value='Create']")).Click();
     
            m_driver.FindElement(By.XPath(".//li//a[contains(text(), 'Manage Payroll')]")).Click();

            m_driver.FindElement(By.XPath(".//table//td[contains(text(), '2020-01-01 12:00:00 AM')]")).Click();
            m_driver.Close();

        }
    }
}