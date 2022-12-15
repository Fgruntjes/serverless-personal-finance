using App.Lib.Dto.Frontend;
using App.Lib.Ynab;
using App.Lib.Ynab.Exception;
using App.Lib.Ynab.Rest;
using Microsoft.AspNetCore.Mvc;

namespace App.Function.Integration.Ynab.Controllers;

[ApiController]
[Route("/status")]
public class StatusController : ControllerBase
{
    private readonly IApiClient _client;

    public StatusController(IApiClient client)
    {
        _client = client;
    }

    [HttpGet(Name = "Status")]
    public async Task<Lib.Dto.Frontend.ApiResponse<IntegrationStatus>> Status()
    {
        try
        {
            var connectedUser = await _client.GetUser();
            return new Lib.Dto.Frontend.ApiResponse<IntegrationStatus>(
                new IntegrationStatus(true, connectedUser.Data.Id)
            );
        }
        catch (TokenException httpException)
        {
            return new Lib.Dto.Frontend.ApiResponse<IntegrationStatus>(new IntegrationStatus(false))
            {
                Errors = new[] { new ApiError(ErrorType.Integration, httpException.Message) }
            };
        }
    }
}