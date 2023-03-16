using Pulumi;

namespace App.Deploy.Component;

interface ICloudFunctionComponent
{
	public Output<string> Url { get; }
}