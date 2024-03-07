using DotnetSelenium.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V120.Network;
using OpenQA.Selenium.Support.UI;
using static DotnetSelenium.SeleniumCustomMethods;

namespace DotnetSelenium;

[TestFixture("admin", "password")] //not used for data driven testing
public class UnitTests
{
    private IWebDriver _driver;
    private readonly string username;
    private readonly string password;

    public UnitTests(string username, string password)
    {
        this.username = username;
        this.password = password;
    }

    [SetUp]
    public void Setup()
    {
        _driver = new ChromeDriver();
        _driver.Navigate().GoToUrl("http://eaapp.somee.com/"); //tutorial testpage
        //_driver.Navigate().GoToUrl("http://test.rubywatir.com/index.php"); //more useful testing page
    }

    [Test]
    public void Test1()
    {
        //3. Maximize the window
        _driver.Manage().Window.Maximize();

        //4. Find the element
        IWebElement webElement = _driver.FindElement(By.Name("q"));

        //5. Type some text in the element
        webElement.SendKeys("Selenium");

        //6. Click on the element
        webElement.SendKeys(Keys.Return);

        // webElement.Click();
    }
    
    [Test]
    public void Login()
    {
        //3. Find and click the login link
        IWebElement loginLink = _driver.FindElement(By.Id("loginLink"));

        //4. Click on the login link
        loginLink.Click();

        //5. Find the UserName input field
        IWebElement txtUserName = _driver.FindElement(By.Name("UserName"));

        //6. Enter a username
        txtUserName.SendKeys(username);

        //7. Find the Password input field
        IWebElement txtPassword = _driver.FindElement(By.Id("Password"));

        //8. Enter a password
        txtPassword.SendKeys(password);

        //9a. Find the login button using ClassName
        // IWebElement btnLogin = _driver.FindElement(By.ClassName("btn"));

        //9b. Find the login button using CssSelector
        IWebElement btnLogin = _driver.FindElement(By.CssSelector(".btn"));

        //10. Click on the login button
        btnLogin.Submit();
    }

    [Test]
    [Category("smoke")] //add specific testing categories to execute them in a group
    public void LoginPOM()
    {
        //3. POM initialization
        LoginPage loginPage = new LoginPage(_driver);

        //4. Click login link
        loginPage.ClickLogin();
        loginPage.Login(username, password);
    }

    [Test]
    public void LoginReduced()
    {
        //3. Find and click the login link
        _driver.FindElement(By.Id("loginLink")).Click();

        //5. Find the UserName input field and enter user name
        _driver.FindElement(By.Name("UserName")).SendKeys(username);

        //7. Find the Password input field and enter password
        _driver.FindElement(By.Id("Password")).SendKeys(password);

        //8. Find and select the Remember me checkbox
        _driver.FindElement(By.Id("RememberMe")).Click();

        //9a. Find the login button using ClassName
        //_driver.FindElement(By.ClassName("btn")).Submit();

        //9b. Find the login button using CssSelector and submit
        _driver.FindElement(By.CssSelector(".btn")).Submit();
    }

    [Test]
    public void AdvancedControls()
    {
        //3. Find and select option from a dropdown list (example)
        SelectElement selectElement = new SelectElement(_driver.FindElement(By.Id("dropdown")));
        selectElement.SelectByText("Option 2");

        //4. Find and select multiple from a select list/dropdown list (example)
        //SelectElement multiSelect = new SelectElement(_driver.FindElement(By.Id("multiselect")));
        //multiSelect.SelectByValue("multi1");
        //multiSelect.SelectByValue("multi2");
        MultiSelectElements(_driver, By.Id("multiselect"), ["multi1", "multi2"]);

        //5. Verify the selected option for the multi select element
        //IList<IWebElement> selectedOption = multiSelect.AllSelectedOptions;
        //foreach (IWebElement option in selectedOption)
        //{
        //    Console.WriteLine(option.Text);
        //}
        List<string> getSelectedOptions = GetAllSelectedElements(_driver, By.Id("multiselect"));
        getSelectedOptions.ForEach(Console.WriteLine);
        //foreach (string selectedOption in getSelectedOptions)
        //{
        //    Console.WriteLine(selectedOption);
        //}

        //6. Find a select a checkbox

    }

    [Test]
    public void CreateEmployeeChatGPT()
    {
        LoginPage loginPage = new LoginPage(_driver);
        loginPage.ClickLogin();
        loginPage.Login(username, password);
        CreateEmployeePage createEmployeePage = new CreateEmployeePage(_driver);
        createEmployeePage.FillEmployeeDetails("test employee 2", "250000", "1000", "Senior", "testemp@gmail.com");
    }

    [Test]
    public void RubyWatirPageForm()
    {
        _driver.FindElement(By.LinkText("Form")).Click();
        // SeleniumCustomMethods.Click(_driver, By.LinkText("Form")); CHANGE after adding POM page
        _driver.FindElement(By.Name("yourname")).SendKeys("Jake");
        _driver.FindElement(By.Name("email")).SendKeys("jake@gmail.com");
        _driver.FindElement(By.XPath("//input[@type='radio' and @value='No']")).Click();
        _driver.FindElement(By.Name("comments")).SendKeys("This is a comment");
        _driver.FindElement(By.XPath("//input[@type='submit' and @value='Send it!']")).Submit();
    }

    [Test]
    public void RubyWatirPageFormCustomMethods()
    {
        // Click(_driver, By.LinkText("Form")); CHANGE after adding POM page
        // Click(_driver, By.LinkText("Form")); CHANGE after adding POM page
        //EnterText(_driver, By.Name("yourname"), "Jake");
        //EnterText(_driver, By.Name("email"), "jake@gmail.com");
        // Click(_driver, By.XPath("//input[@type='radio' and @value='No']")); CHANGE after adding POM page
        //EnterText(_driver, By.Name("comments"), "This is a comment");
        SubmitForm(_driver, By.XPath("//input[@type='submit' and @value='Send it!']"));
        // _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20); Doesn't seem to work for some reason?
    }

    [Test]
    public void RubyWatirFormPOM()
    {
        RubyFormPage FormPage = new RubyFormPage(_driver);
        FormPage.ClickForm();
        FormPage.SubmitForm("John Doe", "johnd@gmail.com", "No", "This is another test.");
    }

    [Test]
    public void RubyWatirPageIfElse()
    {
        IfElseClick(_driver, By.Id("buttonnumber"));
        Assert.That(_driver.FindElement(By.Id("passed")).Text, Is.EqualTo("You clicked the correct button"));
    }

    [Test]
    public void RubyWatirLoopPage()
    {
        MultipleClicks(_driver, By.Name("Submit"), 5);
        // Assert.AreEqual("You have pressed the button 5 times", _driver.FindElement(By.Id("result")).Text); VSCode doesn't seem to like this "classic" style
        Assert.That(_driver.FindElement(By.Id("result")).Text, Is.EqualTo("You have pressed the button 5 times")); //This wasy actually fails the test and shows both values in the test window
    }

    [Test]
    public void RubyWatirMathsPage()
    {

        // IWebElement num1 = _driver.FindElement(By.XPath("//div[@id='1st']//following::div[@id='1stTable']")); 
        IWebElement num1 = _driver.FindElement(By.XPath("//div[@id='1stTable']//following::div[@id='1st']")); 
        int firstnum = Convert.ToInt32(num1.Text);
        IWebElement operand = _driver.FindElement(By.XPath("//div[@id='1stTable']//following::div[@id='symbol']"));
        IWebElement num2 = _driver.FindElement(By.XPath("//div[@id='1stTable']//following::div[@id='2nd']")); 
        int secNum = Convert.ToInt32(num2.Text);
        IWebElement equal = _driver.FindElement(By.XPath("//div[@id='1stTable']//following::td['=']")); 
        IWebElement result = _driver.FindElement(By.XPath("//div[@id='1stTable']//following::div[@id='result']"));
        int answer = firstnum + secNum;

        // int finalCheck = 

        Assert.That(Convert.ToString(answer), Is.EqualTo(result.Text));
    }

    [TearDown]
    public void TearDown()
    {
        Thread.Sleep(2000);
        _driver.Quit();
        _driver.Dispose(); //Not sure if it should come before or after Quit()...
    }
}