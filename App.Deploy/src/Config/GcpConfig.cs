namespace App.Deploy.Config;

public class GcpConfig : ConfigBase
{
    public string Project => _config.Require("project");
    public string Region => _config.Require("region");

    public GcpConfig(string name) : base(name)
    {
    }
}