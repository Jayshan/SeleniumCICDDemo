using DotnetSelenium.Driver;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;

namespace DotnetSelenium;

[TestFixture]
public class SeleniumGridTest
{
    private IWebDriver _driver;

    [SetUp]
    public void Setup()
    {
        _driver = new RemoteWebDriver(new Uri("http://localhost:4444"), new ChromeOptions());
        _driver.Navigate().GoToUrl("http://eaapp.somee.com/");
        _driver.Manage().Window.Maximize();
    }

    //Test case utilizing the POM, DDT using JSON file as data source, and utlizing the Arrange, Act, Assert pattern
    [Test]
    [Category("ddt")]
    [TestCaseSource(nameof(LoginData))]
    public void Login(LoginModel loginModel)
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
    [Category("ddt")]
    [TestCaseSource(nameof(LoginJsonData))]
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

    //Method to create test data source directly in the test file (not ideal, "wrong way")
    public static IEnumerable<LoginModel> LoginData()
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

    [TearDown]
    public void TearDown()
    {
        Thread.Sleep(2000);
        _driver.Quit();
        _driver.Dispose();
    }
}