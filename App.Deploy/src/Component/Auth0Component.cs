using App.Deploy.Config;
using Pulumi;
using Pulumi.Auth0;
using Pulumi.Auth0.Inputs;

namespace App.Deploy.Component;

internal class Auth0Component : ComponentResource
{
	private readonly Dictionary<string, string> _scopes = new()
	{
		{ "function.integration.ynab", "Configure YNAB integration" }
	};
	
	private readonly ResourceServer _publicApi;

	public Auth0Component(
		string name,
		AppConfig config
	) : this(name, config, new ComponentResourceOptions())
	{
	}

	public Auth0Component(
		string name,
		AppConfig config,
		ComponentResourceOptions opts
		) : base("app:auth0", name, opts)
	{
		_publicApi = new ResourceServer(
			$"{name}-public",
			new ResourceServerArgs
			{
				Identifier = $"{name}-public",
				Scopes = GetApiScopes()
			},
			new CustomResourceOptions { Parent = this });
		
		new Role(
			$"{name}-user",
			new()
			{
				Description = "App user",
				Permissions = GetRolePermissions()
			},
			new CustomResourceOptions { Parent = this });
		
		new Rule(
			$"{name}-tenant-claim",
			new()
			{
				Script = $@"
					function (user, context, callback) {{
						if (context.clientName.startsWith('{config.Environment}:')) {{
							context.accessToken['app/tenants'] = user?.app_metadata?.tenants || [];
						}}

						return callback(null, user, context);
				}}"
			},
			new CustomResourceOptions { Parent = this });
		
		RegisterOutputs();
	}

	private InputList<ResourceServerScopeArgs> GetApiScopes()
	{
		return _scopes
			.Select(kvp => new ResourceServerScopeArgs
				{
					Value = kvp.Key,
					Description = kvp.Value
				}
			).ToList();
	}

	private InputList<RolePermissionArgs> GetRolePermissions()
	{
		return _scopes
			.Select(kvp => new RolePermissionArgs
				{
					ResourceServerIdentifier = _publicApi.Identifier,
					Name = kvp.Key
				}
			).ToList();
	}
}