namespace App.Lib.Configuration;

public class Auth0Options
{
    public const string OptionsKey = "Auth0";

    public string Domain { get; set; }
    public string Policies { get; set; }
}