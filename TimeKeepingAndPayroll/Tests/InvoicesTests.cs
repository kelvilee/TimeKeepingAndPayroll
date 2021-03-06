﻿using NUnit.Framework;
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
            m_driver.Url = "http://localhost:51729/";
            m_driver.Manage().Window.Maximize();

            m_driver.FindElement(By.Id("EmployeeID")).SendKeys("1");
            m_driver.FindElement(By.XPath("//input[@value='Login']")).Click();
            m_driver.FindElement(By.XPath(".//li//a[contains(text(), 'Payroll')]")).Click();

            IWebElement table = m_driver.FindElement(By.XPath(".//table[@class='table']"));
            m_driver.Close();
        }

        [Test]
        public void createInvoice()
        {
            m_driver = new ChromeDriver();
            m_driver.Url = "http://localhost:51729";
            m_driver.Manage().Window.Maximize();

            m_driver.FindElement(By.Id("EmployeeID")).SendKeys("2");
            m_driver.FindElement(By.XPath("//input[@value='Login']")).Click();
            m_driver.FindElement(By.XPath(".//li//a[contains(text(), 'Manage Payroll')]")).Click();

            IWebElement createNewInvoiceBtn = m_driver.FindElement(By.XPath("//a[contains(text(),'Create')]"));
            createNewInvoiceBtn.Click();
            var employeeddl = m_driver.FindElement(By.Id("Employee_ID"));
            var selectElement = new SelectElement(employeeddl);
            selectElement.SelectByText("1");

            m_driver.FindElement(By.Id("PayPeriodStart")).SendKeys("01/01/2020");
            m_driver.FindElement(By.Id("PayPeriodEnd")).SendKeys("01/14/2020");

            m_driver.FindElement(By.XPath("//input[@value='Continue']")).Click();

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