namespace App.Deploy.Config;

public class Auth0Config : ConfigBase
{
    public string Domain => _config.Require("domain");
    public string ClientId => _config.Require("client_id");
    public string ClientSecret => _config.Require("client_secret");

    public Auth0Config(string name) : base(name)
    {
    }
}