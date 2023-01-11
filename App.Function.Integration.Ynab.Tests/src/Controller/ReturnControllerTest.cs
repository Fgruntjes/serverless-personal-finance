using System.Net;
using App.Lib.Dto.Frontend;
using App.Lib.Tests;
using App.Lib.Ynab.Exception;
using App.Lib.Ynab.Rest.Dto;
using Moq;

namespace App.Function.Integration.Ynab.Tests.Controller;

public class ReturnControllerTest : ControllerTest
{
    public ReturnControllerTest(TestApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async void MissingCode()
    {
        (await _client.GetAsync("/return"))
            .Should()
            .HaveStatusCode(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async void ValidCode()
    {
        const string returnCode = "123";

        _mockedConnectService
            .Setup(c => c.ProcessReturn(returnCode));

        var apiClientUserResponse = new Lib.Ynab.Rest.Dto.ApiResponse<UserData>
        {
            Data = new UserData
            {
                Id = "some.user@example.com",
            }
        };
        _mockedClient
            .Setup(c => c.GetUser())
            .ReturnsAsync(apiClientUserResponse);

        (await _client.GetFromJsonAsync<Lib.Dto.Frontend.ApiResponse<IntegrationStatus>>($"/return?code={returnCode}"))
            .Should()
            .BeEquivalentTo(new Lib.Dto.Frontend.ApiResponse<IntegrationStatus>(new IntegrationStatus(true, apiClientUserResponse.Data.Id)));

        _mockedConnectService.Verify(c => c.ProcessReturn(returnCode), Times.Once());
    }

    [Fact]
    public async void ProcessException()
    {
        _mockedConnectService
            .Setup(c => c.ProcessReturn(It.IsAny<string>()))
            .Throws(new TokenException("Some token failure."));

        (await _client.GetFromJsonAsync<Lib.Dto.Frontend.ApiResponse<IntegrationStatus>>("/return?code=123"))
            .Should()
            .BeEquivalentTo(new Lib.Dto.Frontend.ApiResponse<IntegrationStatus>(new IntegrationStatus(false))
            {
                Errors = new[] { new AppApiError(ErrorType.Integration, "Some token failure.") }
            });
    }

    [Fact]
    public async void GetUserException()
    {
        _mockedConnectService
            .Setup(c => c.ProcessReturn(It.IsAny<string>()));

        _mockedClient
            .Setup(c => c.GetUser())
            .Throws(new TokenException("Some token failure."));

        (await _client.GetFromJsonAsync<Lib.Dto.Frontend.ApiResponse<IntegrationStatus>>("/return?code=123"))
            .Should()
            .BeEquivalentTo(new Lib.Dto.Frontend.ApiResponse<IntegrationStatus>(new IntegrationStatus(false))
            {
                Errors = new[] { new AppApiError(ErrorType.Integration, "Some token failure.") }
            });
    }
}