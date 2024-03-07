namespace DotnetSelenium;

public class LoginModel
{
    public required string userName { get; set; } //complained about being "non-nullable" so one fix was to add the "required" attribute
    public required string passWord { get; set; }
}