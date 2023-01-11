using App.Lib.Dto.Frontend;
using App.Lib.Tests;
using App.Lib.Ynab;
using App.Lib.Ynab.Rest.Dto;
using Moq;

namespace App.Function.Integration.Ynab.Tests.Controller;

public class StatusControllerTest : ControllerTest
{
    public StatusControllerTest(TestApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async void Connected()
    {
        _mockedConnectService
            .Setup(c => c.IsConnected())
            .ReturnsAsync(true);

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
    public async void Disconnected()
    {
        _mockedConnectService
            .Setup(c => c.IsConnected())
            .ReturnsAsync(false);

        (await _client.GetFromJsonAsync<Lib.Dto.Frontend.ApiResponse<IntegrationStatus>>("/status"))
            .Should()
            .BeEquivalentTo(new Lib.Dto.Frontend.ApiResponse<IntegrationStatus>(new IntegrationStatus())
            { Data = new IntegrationStatus { Connected = false } });
    }

    [Fact]
    public async void Connected_TokenNotSetException()
    {
        _mockedConnectService
            .Setup(c => c.IsConnected())
            .ReturnsAsync(true);

        _mockedClient
            .Setup(c => c.GetUser())
            .Throws<TokenNotSetException>();

        (await _client.GetFromJsonAsync<Lib.Dto.Frontend.ApiResponse<IntegrationStatus>>("/status"))
            .Should()
            .BeEquivalentTo(new Lib.Dto.Frontend.ApiResponse<IntegrationStatus>(new IntegrationStatus())
            {
                Data = new IntegrationStatus { Connected = false },
                Errors = new[] { new AppApiError(ErrorType.Integration, "YNAB access token not set.") }
            });
    }

    [Fact]
    public async void Connected_NotAuthorizedHttpException()
    {
        _mockedConnectService
            .Setup(c => c.IsConnected())
            .ReturnsAsync(true);

        _mockedClient
            .Setup(c => c.GetUser())
            .Throws<TokenNotSetException>();

        (await _client.GetFromJsonAsync<Lib.Dto.Frontend.ApiResponse<IntegrationStatus>>("/status"))
            .Should()
            .BeEquivalentTo(new Lib.Dto.Frontend.ApiResponse<IntegrationStatus>(new IntegrationStatus())
            {
                Errors = new[] { new AppApiError(ErrorType.Integration, "YNAB access token not set.") }
            });
    }
}