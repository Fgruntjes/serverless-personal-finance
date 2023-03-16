using Pulumi;

namespace App.Deploy.Config;

public class SentryConfig : ConfigBase
{
	public string Dsn => _config.Require("dsn");
	
	public SentryConfig(string name) : base(name)
	{
	}
}