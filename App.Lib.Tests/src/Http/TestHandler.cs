using System.Net;

namespace App.Lib.Tests.Http;

public class TestHandler : DelegatingHandler
{
	public readonly IList<HttpRequestMessage> Requests;
	private readonly Func<HttpRequestMessage,
		CancellationToken, Task<HttpResponseMessage>> _handlerFunc;

	public TestHandler() : this((_, _) => Return200())
	{}

	public TestHandler(Func<HttpRequestMessage,
		CancellationToken, Task<HttpResponseMessage>> handlerFunc)
	{
		_handlerFunc = handlerFunc;
		Requests = new List<HttpRequestMessage>();
	}

	protected override Task<HttpResponseMessage> SendAsync(
		HttpRequestMessage request, CancellationToken cancellationToken)
	{
		Requests.Add(request);
		return _handlerFunc(request, cancellationToken);              
	}

	private static Task<HttpResponseMessage> Return200()
	{
		return Task.Factory.StartNew(
			() => new HttpResponseMessage(HttpStatusCode.OK));
	}
}