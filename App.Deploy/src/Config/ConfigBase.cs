namespace App.Deploy.Config;

public class ConfigBase
{
	protected readonly Pulumi.Config _config;
	
	public ConfigBase(string name)
	{
		_config = new Pulumi.Config(name);
	}
}