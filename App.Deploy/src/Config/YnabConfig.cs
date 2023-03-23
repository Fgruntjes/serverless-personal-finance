namespace App.Deploy.Config;

public class YnabConfig : ConfigBase
{
    public string ClientId => _config.Require("clientId");
    public string ClientSecret => _config.Require("clientSecret");

    public YnabConfig(string name) : base(name)
    {
    }
}