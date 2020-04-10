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

namespace TimeKeepingAndPayroll.Tests
{
    class AttendanceTests
    {
        IWebDriver driver;

        [SetUp]
        public void startBrowser()
        {
            driver = new ChromeDriver(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\Tests"));
            driver.Url = "http://localhost:51729/"; // you will need to run IIS Express on another instance of VS and run these tests on this one, make sure this matches your localhost:PORT# of the running instance of IIS Express
        }

        [Test]
        public void createCustomer()
        {
            //IWebElement customerNavBtn = driver.FindElement(By.XPath(".//*[@class='navbar navbar-inverse navbar-fixed-top']//div/div[2]/ul/li[2]/a"));
            //customerNavBtn.Click();
            //IWebElement createCustomer = driver.FindElement(By.XPath(".//*[@class='container body-content']//p/a"));
            //createCustomer.Click();
            //IWebElement fNameTextBox = driver.FindElement(By.XPath(".//*[@id='FirstName']"));
            //IWebElement createBtn = driver.FindElement(By.XPath(".//*[@class='btn btn-default']"));
            //fNameTextBox.SendKeys("Jake");
            //createBtn.Click();
        }

        //[Test]
        //public void createCustomerContact()
        //{
        //    IWebElement customerNavBtn = driver.FindElement(By.XPath(".//*[@class='navbar navbar-inverse navbar-fixed-top']//div/div[2]/ul/li[2]/a"));
        //    customerNavBtn.Click();
        //    IWebElement createCustomerContact = driver.FindElement(By.XPath(".//*[@class='table']//tbody/tr[2]/td[3]/a[4]"));
        //    createCustomerContact.Click();
        //    IWebElement createBtn = driver.FindElement(By.XPath(".//*[@class='container body-content']//p/a"));
        //    createBtn.Click();
        //    IWebElement fNameTextBox = driver.FindElement(By.XPath(".//*[@id='FirstName']"));
        //    IWebElement insertBtn = driver.FindElement(By.XPath(".//*[@class='btn btn-default']"));
        //    IWebElement customer = driver.FindElement(By.XPath(".//*[@id='CustomerID']"));
        //    var selectTest = new SelectElement(customer);
        //    // Select a value from the dropdown				
        //    selectTest.SelectByValue("0b0f84bf-a1d2-42c1-8519-d3ab046125dc");
        //    fNameTextBox.SendKeys("Jake");
        //    insertBtn.Click();
        //}

        //[Test]
        //public void delCustomerContact()
        //{
        //    IWebElement customerNavBtn = driver.FindElement(By.XPath(".//*[@class='navbar navbar-inverse navbar-fixed-top']//div/div[2]/ul/li[2]/a"));
        //    customerNavBtn.Click();
        //    IWebElement openCustomerContact = driver.FindElement(By.XPath(".//*[@class='table']//tbody/tr[2]/td[3]/a[4]"));
        //    openCustomerContact.Click();
        //    IWebElement delCustomer = driver.FindElement(By.XPath(".//*[@class='table']//tbody/tr[2]/td[4]/a[1]"));
        //    delCustomer.Click();
        //    IWebElement delBtn = driver.FindElement(By.XPath(".//*[@class='btn btn-default']"));
        //    delBtn.Click();
        //}

        //[Test]
        //public void updateCustomer()
        //{
        //    IWebElement customerNavBtn = driver.FindElement(By.XPath(".//*[@class='navbar navbar-inverse navbar-fixed-top']//div/div[2]/ul/li[2]/a"));
        //    customerNavBtn.Click();
        //    IWebElement editCustomer = driver.FindElement(By.XPath(".//*[@class='table']//tbody/tr[2]/td[3]/a[1]"));
        //    editCustomer.Click();
        //    IWebElement descTextBox = driver.FindElement(By.XPath(".//*[@id='Description']"));
        //    IWebElement saveBtn = driver.FindElement(By.XPath(".//*[@class='btn btn-default']"));
        //    descTextBox.SendKeys("yare yare darze");
        //    saveBtn.Click();
        //}

        //[Test]
        //public void deleteCustomer()
        //{
        //    IWebElement customerNavBtn = driver.FindElement(By.XPath(".//*[@class='navbar navbar-inverse navbar-fixed-top']//div/div[2]/ul/li[2]/a"));
        //    customerNavBtn.Click();
        //    IWebElement delCustomer = driver.FindElement(By.XPath(".//*[@class='table']//tbody/tr[2]/td[3]/a[3]"));
        //    delCustomer.Click();
        //    IWebElement delBtn = driver.FindElement(By.XPath(".//*[@class='btn btn-default']"));
        //    delBtn.Click();
        //}

        [TearDown]
        public void closeBrowser()
        {
            driver.Close();
        }

    }
}