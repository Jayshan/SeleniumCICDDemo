using OpenQA.Selenium.Support.UI;

namespace DotnetSelenium
{
    public static class SeleniumCustomMethods
    {
        //1. Method should get the locator
        //2. Get the type of identifier
        //3. Perform action on locator
        public static void ClickElement(this IWebElement locator)
        {
            locator.Click();
        }

        public static void SubmitElement(this IWebElement locator)
        {
            locator.Submit();
        }

        public static void EnterText(this IWebElement locator, string text)
        {
            locator.Clear();
            locator.SendKeys(text);
        }

        public static void SubmitForm(IWebDriver driver, By locator)
        {
            driver.FindElement(locator).Submit();
        }

        public static void SelectDropDownByText(IWebDriver driver, By locator, string text)
        {
            SelectElement selectElement = new SelectElement(driver.FindElement(locator));
            selectElement.SelectByText(text);
        }

        public static void SelectDropDownByValue(IWebDriver driver, By locator, string value)
        {
            SelectElement selectElement = new SelectElement(driver.FindElement(locator));
            selectElement.SelectByValue(value);
        }

        public static void MultiSelectElements(IWebDriver driver, By locator, string[] values)
        {
            SelectElement multiSelect = new SelectElement(driver.FindElement(locator));
            foreach (string value in values)
            {
                multiSelect.SelectByValue(value);
            }
        }

        public static List<string> GetAllSelectedElements(IWebDriver driver, By locator)
        {
            List<string> options = new List<string>();
            SelectElement selectElement = new SelectElement(driver.FindElement(locator));
            IList<IWebElement> selectedOption = selectElement.AllSelectedOptions;
            foreach (IWebElement option in selectedOption)
            {
                options.Add(option.Text);
            }
            return options;
        }

        public static void IfElseClick(IWebDriver driver, By locator)
        {
            switch (Convert.ToInt32(driver.FindElement(locator).Text)) 
            {
                case 1:
                    driver.FindElement(By.Name("1st")).Submit();
                    break;
                case 2:
                    driver.FindElement(By.Name("2nd")).Submit();
                    break;
                case 3:
                    driver.FindElement(By.Name("3rd")).Submit();
                    break;
                case 4:
                    driver.FindElement(By.Name("4th")).Submit();
                    break;
                case 5:
                    driver.FindElement(By.Name("5th")).Submit();
                    break;
            }
        }

        public static void MultipleClicks(IWebDriver driver, By locator, int mulitple)
        {
            for (int i = 0; i < mulitple; i++)
            {
                driver.FindElement(locator).Submit();
                // Thread.Sleep(2000);
                // driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
            }
        }
    }
}