using App.Lib.Dto.Frontend;
using App.Lib.Tests;
using App.Lib.Ynab;
using App.Lib.Ynab.Rest;
using App.Lib.Ynab.Rest.Dto;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace App.Function.Integration.Ynab.Tests.Controller;

public class StatusControllerTest : IntegrationTestFixture<Program>
{
    private readonly Mock<IApiClient> _mockedClient;

    public StatusControllerTest(TestApplicationFactory<Program> factory) : base(factory)
    {
        _mockedClient = new Mock<IApiClient>();
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(s => s.AddScoped(_ => _mockedClient.Object));
        }).CreateClient();
    }

    [Fact]
    public async void StatusWhenConnected()
    {
        var getUserResponse = new Lib.Ynab.Rest.Dto.ApiResponse<UserData>
        {
            Data = new UserData
            {
                Id = "some.user@example.com",
            }
        };
        _mockedClient
            .Setup(c => c.GetUser())
            .ReturnsAsync(getUserResponse);

        (await _client.GetFromJsonAsync<Lib.Dto.Frontend.ApiResponse<IntegrationStatus>>("/status"))
            .Should()
            .BeEquivalentTo(new Lib.Dto.Frontend.ApiResponse<IntegrationStatus>(new IntegrationStatus(true, getUserResponse.Data.Id)));
    }

    [Fact]
    public async void VoidWhenNotConnected()
    {
        _mockedClient
            .Setup(c => c.GetUser())
            .Throws<TokenNotSetException>();

        (await _client.GetFromJsonAsync<Lib.Dto.Frontend.ApiResponse<IntegrationStatus>>("/status"))
            .Should()
            .BeEquivalentTo(new Lib.Dto.Frontend.ApiResponse<IntegrationStatus>(new IntegrationStatus())
            {
                Errors = new[] { new ApiError(ErrorType.Integration, "YNAB access token not set.") }
            });
    }
}