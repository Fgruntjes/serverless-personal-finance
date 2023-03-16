namespace App.Deploy.Config;

public class CloudflareConfig : ConfigBase
{
	public string AccountId => _config.Require("accountId");

	public CloudflareConfig(string name) : base(name)
	{
		
	}
}