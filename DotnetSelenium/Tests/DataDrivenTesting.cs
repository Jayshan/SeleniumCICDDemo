using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using DotnetSelenium.Driver;
using DotnetSelenium.Models;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Safari;

namespace DotnetSelenium;

[TestFixture]
public class DataDrivenTesting
{
    private IWebDriver _driver;
    private ExtentReports _extentreport;
    private ExtentTest _extenttest;
    private ExtentTest _testnode;

    [SetUp]
    public void Setup()
    {
        _driver = GetWebDriver(DriverType.Chrome);
        SetupExtentReports();
        _testnode = _extenttest.CreateNode("Setup and TearDown Info").Pass("Browser launched");
        _driver.Navigate().GoToUrl("http://eaapp.somee.com/"); //tutorial testpage
        //_driver.Navigate().GoToUrl("http://test.rubywatir.com/index.php"); //more useful testing page
        _driver.Manage().Window.Maximize();
    }

    private IWebDriver GetWebDriver(DriverType driverType)
    {
        switch (driverType) 
        {
            case DriverType.Chrome:
                return new ChromeDriver();
            case DriverType.Firefox:
                return new FirefoxDriver();
            case DriverType.Edge:
                return new EdgeDriver();
            case DriverType.Safari:
                return new SafariDriver();
            default:
                return new ChromeDriver();
        }
    }

    private void SetupExtentReports()
    {
        _extentreport = new ExtentReports();
        var spark = new ExtentSparkReporter("TestExtentReport.html");
        _extentreport.AttachReporter(spark);
        _extentreport.AddSystemInfo("OS", "MacOS");
        _extentreport.AddSystemInfo("Browser", DriverType.Chrome.ToString());
        _extenttest = _extentreport.CreateTest("Login Test Implicit Wait").Log(Status.Info, "First extent report initialized.");    
    }

    //Test case utilizing the POM, DDT using JSON file as data source, and utlizing the Arrange, Act, Assert pattern
    [Test]
    [Category("ddt")]
    [TestCaseSource(nameof(LoginJsonData))]
    public void LoginDDT(LoginModel loginModel)
    {
        //Arrange
        LoginPage loginPage = new LoginPage(_driver);
        
        //Act
        loginPage.ClickLogin();
        loginPage.Login(loginModel.userName, loginModel.passWord);
        
        //Assert using Tuples
        var getLoggedIn = loginPage.IsLoggedIn(); //(bool loginlnk, bool manageusrs) getLoggedIn = loginPage.IsLoggedIn();
        Assert.IsTrue(getLoggedIn.loginlnk && getLoggedIn.manageusrs);
    }

    //Test case utilizing the POM, DDT using JSON file as data source, utlizing the Arrange, Act, Assert pattern, and Fluent Assertions
    [Test]
    [Category("GHDemo")]
    [TestCaseSource(nameof(Login))]
    public void LoginFluentAssertions(LoginModel loginModel)
    {
        //Arrange
        LoginPage loginPage = new LoginPage(_driver);
        
        //Act
        loginPage.ClickLogin();
        loginPage.Login(loginModel.userName, loginModel.passWord);
        
        //Assert using Fluent Assertions
        var getLoggedIn = loginPage.IsLoggedIn(); //(bool loginlnk, bool manageusrs) getLoggedIn = loginPage.IsLoggedIn();
        getLoggedIn.loginlnk.Should().BeTrue();
        getLoggedIn.manageusrs.Should().BeTrue();

    
    }
    //Test case utilizing the POM, DDT using JSON file as data source, utlizing the Arrange, Act, Assert pattern, Fluent Assertions, and Implicit Waits
    [Test]
    [Category("ddt")]
    [TestCaseSource(nameof(Login))]
    public void LoginImplicitWait(LoginModel loginModel)
    {
        //Arrange
        LoginPage loginPage = new LoginPage(_driver);
        
        //Act
        loginPage.ClickLogin();
        _extenttest.Log(Status.Pass, "Click Login");
        loginPage.Login(loginModel.userName, loginModel.passWord);
        _extenttest.Log(Status.Pass, "Username and Password entered");
        
        //Assert using Fluent Assertions
        var getLoggedIn = loginPage.IsLoggedIn(); //(bool loginlnk, bool manageusrs) getLoggedIn = loginPage.IsLoggedIn();
        getLoggedIn.loginlnk.Should().BeTrue();
        getLoggedIn.manageusrs.Should().BeTrue();
        _extenttest.Log(Status.Pass, "Logged in successfully");
    }

    //Test case utilizing the POM, but parsing the JSON file data in the test itself (instead of using the data as an "external source")
    [Test]
    [Category("ddt")]
    public void LoginDDTwithJSONData()
    {
        string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "login.json");
        string jsonString = File.ReadAllText(jsonFilePath);
        var loginModel = JsonSerializer.Deserialize<LoginModel>(jsonString);
        
        LoginPage loginPage = new LoginPage(_driver);
        loginPage.ClickLogin();
        loginPage.Login(loginModel.userName, loginModel.passWord);
    }

    //Method to create test data source directly in the test file (not ideal, "wrong way")
    public static IEnumerable<LoginModel> Login()
    {
        yield return new LoginModel()
        {
            userName = "admin",
            passWord = "password"
        };
    }

    //Method to create data source by parsing data from a JSON file, and testing using each individual array item (correct way)
     public static IEnumerable<LoginModel> LoginJsonData()
    {
        string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "login.json");
        string jsonString = File.ReadAllText(jsonFilePath);
        var loginModel = JsonSerializer.Deserialize<List<LoginModel>>(jsonString);

        foreach (var loginData in loginModel)
        {
            yield return loginData;
        }
    }

    //Method to read and parse data from a JSON file (nothing is returned)
    private void ReadJSONFile()
    {
        //jsonFilePath string will contain complete file path of login.json, including base directpry path
        string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "login.json");
        //Retrieve JSON data in string format
        string jsonString = File.ReadAllText(jsonFilePath);
        //Parse JSON string data (we need to convert it to a "LoginModel" type) DESERIALIZE
        var loginModel = JsonSerializer.Deserialize<LoginModel>(jsonString);
    }

    [TearDown]
    public void TearDown()
    {
        Thread.Sleep(2000);
        _driver.Quit();
        _driver.Dispose();
        _testnode.Pass("Browser quit");
        _extentreport.Flush();
    }

}