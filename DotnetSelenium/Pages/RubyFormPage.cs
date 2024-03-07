using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenQA.Selenium;
using static DotnetSelenium.SeleniumCustomMethods;

namespace DotnetSelenium.Pages
{
    public class RubyFormPage
    {
        private readonly IWebDriver driver;

        public RubyFormPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        IWebElement FormLink => driver.FindElement(By.LinkText("Form"));
        IWebElement YourName => driver.FindElement(By.Name("yourname"));
        IWebElement Email => driver.FindElement(By.Name("email"));
        IWebElement RadioBtnYes => driver.FindElement(By.XPath("//input[@type='radio' and @value='Yes']"));
        IWebElement RadioBtnNo => driver.FindElement(By.XPath("//input[@type='radio' and @value='No']"));
        IWebElement RadioBtnNotSure => driver.FindElement(By.XPath("//input[@type='radio' and @value='Not sure']"));
        IWebElement Comments => driver.FindElement(By.Name("comments"));
        IWebElement SendItBtn => driver.FindElement(By.XPath("//input[@type='submit' and @value='Send it!']"));
        
        public void ClickForm()
        {
            FormLink.ClickElement();
        }

        public void SubmitForm(string name, string email, string radio, string comment)
        {
            YourName.EnterText(name);
            Email.EnterText(email);
            if (radio == "Yes")
            {
                RadioBtnYes.ClickElement();
            }
            else if (radio == "No")
            {
                RadioBtnNo.ClickElement();
            }
            else
            {
                RadioBtnNotSure.ClickElement();
            }
            Comments.EnterText(comment);
            SendItBtn.SubmitElement();
        }

    }
}