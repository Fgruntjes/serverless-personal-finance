using App.Deploy.Config;
using Pulumi;
using Pulumi.Mongodbatlas;
using Pulumi.Mongodbatlas.Inputs;
using Pulumi.Random;

namespace App.Deploy.Component;

public class DatabaseComponent : ComponentResource
{
	private readonly DatabaseUser _user;
	private readonly ServerlessInstance _server;

	[Output]
	public Output<string> ConnectionString
	{
		get
		{
			return _server.ConnectionStringsStandardSrv
				.Apply(str => str.Replace("mongodb+srv://", $"mongodb+srv://{_user.Username}:{_user.Password}@"));
		}
		// ReSharper disable once UnusedMember.Global
		set { }
	}

	[Output]
	public Output<string>  DatabaseName
	{
		get => _server.Name;
		// ReSharper disable once UnusedMember.Global
		set {}
	}

	public DatabaseComponent(
		string name,
		AppConfig config
	) : this(name, config, new ComponentResourceOptions())
	{
	}

	public DatabaseComponent(
		string name,
		AppConfig config,
		ComponentResourceOptions opts
	) : base("app:database", name, opts)
	{
		_server = new ServerlessInstance(
			name,
			new ServerlessInstanceArgs
			{
				ProjectId = config.MongoDb.ProjectId,
				ProviderSettingsBackingProviderName = "GCP",
				ProviderSettingsProviderName = "GCP",
				ProviderSettingsRegionName = config.Gcp.Region.Replace(
					"europe-west1",
					"WESTERN_EUROPE")
			},
			new CustomResourceOptions { Parent = this });

		
		new ProjectIpAccessList(name, new()
		{
			Comment = "Public access",
			CidrBlock = "0.0.0.0/0",
			ProjectId = config.MongoDb.ProjectId,
		}, new CustomResourceOptions { Parent = this });

		var password = new RandomPassword($"{name}-password", new()
		{
			Length = 20,
			Special = false,
		}, new CustomResourceOptions { Parent = this });

		_user = new DatabaseUser($"{name}-admin", new()
		{
			ProjectId = config.MongoDb.ProjectId,
			Scopes = new[]
			{
				new DatabaseUserScopeArgs
				{
					Name = config.Environment,
					Type = "CLUSTER",
				}
			},
			Roles = new[]
			{
				new DatabaseUserRoleArgs
				{
					DatabaseName = "admin",
					RoleName = "atlasAdmin",
				},
			},
			AuthDatabaseName = "admin",
			Username = "admin",
			Password = password.Result,
		}, new CustomResourceOptions { Parent = this });
		
		RegisterOutputs();
	}
}