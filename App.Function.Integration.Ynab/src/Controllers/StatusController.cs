using System.Net;
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
    private readonly IConnectService _connectService;

    public StatusController(IApiClient client, IConnectService connectService)
    {
        _client = client;
        _connectService = connectService;
    }

    [HttpGet(Name = "Status")]
    public async Task<ActionResult> Status()
    {
        try
        {
            if (await _connectService.IsConnected())
            {
                var connectedUser = await _client.GetUser();
                return Ok(new ApiResponse<IntegrationStatus>(
                    new IntegrationStatus(true, connectedUser.Data.Id)
                ));
            }

            return Ok(new ApiResponse<IntegrationStatus>(
                new IntegrationStatus(false)
            ));
        }
        catch (Refit.ApiException exception)
        {
            return BadRequest(new ApiResponse<IntegrationStatus>(new IntegrationStatus(false))
            {
                Errors = new[] { new AppApiError(ErrorType.Integration, exception.Message) }
            });
        }
        catch (TokenException exception)
        {
            return BadRequest(new ApiResponse<IntegrationStatus>(new IntegrationStatus(false))
            {
                Errors = new[] { new AppApiError(ErrorType.Integration, exception.Message) }
            });
        }
    }
}