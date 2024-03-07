using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace DotnetSelenium.Pages
{
    
    public class CreateEmployeePage
    {
        private IWebDriver driver;

        // Element locators
        private IWebElement EmployeeListLink => driver.FindElement(By.LinkText("Employee List"));
        private IWebElement CreateNewLink => driver.FindElement(By.LinkText("Create New"));
        private IWebElement NameField => driver.FindElement(By.Id("Name"));
        private IWebElement SalaryField => driver.FindElement(By.Id("Salary"));
        private IWebElement DurationField => driver.FindElement(By.Id("DurationWorked"));
        private IWebElement GradeDropdown => driver.FindElement(By.Id("Grade"));
        private IWebElement EmailField => driver.FindElement(By.Id("Email"));
        private IWebElement SubmitButton => driver.FindElement(By.CssSelector(".btn"));

        // Constructor
        public CreateEmployeePage(IWebDriver driver)
        {
            this.driver = driver;
        }

        // Method for performing all actions together
        public void FillEmployeeDetails(string name, string salary, string duration, string grade, string email)
        {
            // Click "Employee List" link
            EmployeeListLink.Click();

            // Click "Create New" link
            CreateNewLink.Click();

            // Enter Name
            NameField.Click();
            NameField.SendKeys(name);

            // Enter Salary
            SalaryField.Click();
            SalaryField.SendKeys(salary);

            // Enter Duration Worked
            DurationField.Click();
            DurationField.SendKeys(duration);

            // Select Grade
            GradeDropdown.Click();
            GradeDropdown.FindElement(By.XPath($"//option[. = '{grade}']")).Click();

            // Enter Email
            EmailField.Click();
            EmailField.SendKeys(email);

            // Click the submit button
            SubmitButton.Click();
        }
    }

}