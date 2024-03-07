namespace DotnetSelenium.Pages;

public class LoginPage(IWebDriver driver) //primary contructor can pass parameter directly into class, replacing constructor below
{
    //Prior to C# version 12
    // private readonly IWebDriver driver;

    // public LoginPage(IWebDriver driver)
    // {
    //     this.driver = driver;
    // }
    
    IWebElement LoginLink => driver.FindElement(By.Id("loginLink"));
    IWebElement TxtUser => driver.FindElement(By.Id("UserName"));
    IWebElement TxtPassword => driver.FindElement(By.Id("Password"));
    IWebElement BtnLogin => driver.FindElement(By.CssSelector(".btn"));

    //Getting the Log off link element to assert login test (not technically part of login page)
    IWebElement LogOffLink => driver.FindElement(By.LinkText("Log off"));
    //Getting the Manage Users link element to assert login test (not technically part of login page)
    IWebElement ManageUsers => driver.FindElement(By.LinkText("Manage Users"));

    public void ClickLogin()
    {
        LoginLink.ClickElement();
    }

    public void Login(string username, string password)
    {
        TxtUser.EnterText(username);
        TxtPassword.EnterText(password);
        BtnLogin.SubmitElement();
    }

    public (bool loginlnk, bool manageusrs) IsLoggedIn()
    {
        return (LogOffLink.Displayed, ManageUsers.Displayed);
    }
}