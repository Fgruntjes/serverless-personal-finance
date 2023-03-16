namespace App.Deploy.Config;

public class MongoDbConfig : ConfigBase
{
	public string ProjectId => _config.Require("projectId");

	public MongoDbConfig(string name) : base(name)
	{
		
	}
}