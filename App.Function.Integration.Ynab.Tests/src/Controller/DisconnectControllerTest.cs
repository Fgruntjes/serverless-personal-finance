using System.Net;
using App.Lib.Dto.Frontend;
using App.Lib.Tests;
using App.Lib.Ynab;
using Moq;

namespace App.Function.Integration.Ynab.Tests.Controller;

public class DisconnectControllerTest : ControllerTest
{
    public DisconnectControllerTest(TestApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async void DisconnectWhenConnected()
    {
        _mockedConnectService
            .Setup(c => c.IsConnected())
            .ReturnsAsync(true);

        var response = await _client.GetAsync("/disconnect");
        response.Should().BeSuccessful();

        _mockedConnectService.Verify(s => s.Disconnect(), Times.Once());
    }

    [Fact]
    public async void DisconnectWhenDisconnected()
    {
        _mockedConnectService
            .Setup(c => c.IsConnected())
            .ReturnsAsync(false);

        var connectResponse = await _client.GetAsync("/disconnect");
        connectResponse
            .Should()
            .HaveStatusCode(HttpStatusCode.BadRequest);

        var apiResponse = await connectResponse.Content.ReadFromJsonAsync<ApiResponse<string>>();
        apiResponse.Should()
            .BeEquivalentTo(new ApiResponse()
            {
                Errors = new[] { new AppApiError(ErrorType.BadRequest, "Not connected.") }
            });
    }
}