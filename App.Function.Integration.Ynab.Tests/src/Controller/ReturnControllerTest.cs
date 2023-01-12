using System.Net;
using App.Lib.Dto.Frontend;
using App.Lib.Tests;
using App.Lib.Ynab.Exception;
using App.Lib.Ynab.Rest.Dto;
using Microsoft.AspNetCore.WebUtilities;
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
        const string returnUrl = "http://localhost:3000/return";

        _mockedConnectService
            .Setup(c => c.ProcessReturn(returnCode, returnUrl));

        var apiClientUserResponse = new Lib.Ynab.Rest.Dto.ApiResponse<UserResponse>
        {
            Data = new UserResponse
            {
                User = new UserResponse.UserData
                {
                    Id = "some.user@example.com",
                }
            }
        };
        _mockedClient
            .Setup(c => c.GetUser())
            .ReturnsAsync(apiClientUserResponse);

        (await _client.GetFromJsonAsync<Lib.Dto.Frontend.ApiResponse<IntegrationStatus>>(QueryHelpers.AddQueryString(
                "/return",
                new Dictionary<string, string>
                {
                    ["returnUrl"] = returnUrl,
                    ["code"] = returnCode
                }
            )))
            .Should()
            .BeEquivalentTo(new Lib.Dto.Frontend.ApiResponse<IntegrationStatus>(new IntegrationStatus(true, apiClientUserResponse.Data.User.Id)));

        _mockedConnectService.Verify(c => c.ProcessReturn(returnCode, returnUrl), Times.Once());
    }

    [Fact]
    public async void ProcessException()
    {
        const string returnCode = "123";
        const string returnUrl = "http://localhost:3000/return";

        _mockedConnectService
            .Setup(c => c.ProcessReturn(returnCode, returnUrl))
            .Throws(new TokenException("Some token failure."));

        (await _client.GetFromJsonAsync<Lib.Dto.Frontend.ApiResponse<IntegrationStatus>>(QueryHelpers.AddQueryString(
                "/return",
                new Dictionary<string, string>
                {
                    ["returnUrl"] = returnUrl,
                    ["code"] = returnCode
                }
            )))
            .Should()
            .BeEquivalentTo(new Lib.Dto.Frontend.ApiResponse<IntegrationStatus>(new IntegrationStatus(false))
            {
                Errors = new[] { new AppApiError(ErrorType.Integration, "Some token failure.") }
            });
    }

    [Fact]
    public async void GetUserException()
    {
        const string returnCode = "123";
        const string returnUrl = "http://localhost:3000/return";

        _mockedConnectService
            .Setup(c => c.ProcessReturn(returnCode, returnUrl));

        _mockedClient
            .Setup(c => c.GetUser())
            .Throws(new TokenException("Some token failure."));

        (await _client.GetFromJsonAsync<Lib.Dto.Frontend.ApiResponse<IntegrationStatus>>(QueryHelpers.AddQueryString(
                "/return",
                new Dictionary<string, string>
                {
                    ["returnUrl"] = returnUrl,
                    ["code"] = returnCode
                }
            )))
            .Should()
            .BeEquivalentTo(new Lib.Dto.Frontend.ApiResponse<IntegrationStatus>(new IntegrationStatus(false))
            {
                Errors = new[] { new AppApiError(ErrorType.Integration, "Some token failure.") }
            });
    }
}