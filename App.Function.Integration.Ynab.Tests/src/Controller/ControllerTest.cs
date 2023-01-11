using App.Lib.Tests;
using App.Lib.Ynab;
using App.Lib.Ynab.Rest;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace App.Function.Integration.Ynab.Tests.Controller;

public class ControllerTest : IntegrationTestFixture<Program>
{
    protected readonly Mock<IApiClient> _mockedClient;
    protected readonly Mock<IConnectService> _mockedConnectService;

    public ControllerTest(TestApplicationFactory<Program> factory) : base(factory)
    {
        _mockedClient = new Mock<IApiClient>();
        _mockedConnectService = new Mock<IConnectService>();
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(s => s.AddScoped(_ => _mockedClient.Object));
            builder.ConfigureTestServices(s => s.AddScoped(_ => _mockedConnectService.Object));
        }).CreateClient();
    }
}