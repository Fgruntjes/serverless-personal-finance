using App.Lib.Ynab;
using App.Lib.Dto.Frontend;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Function.Integration.Ynab.Controllers;

[ApiController]
[Route("/disconnect")]
public class DisconnectController : ControllerBase
{
    private readonly IConnectService _connectService;

    public DisconnectController(IConnectService connectService)
    {
        _connectService = connectService;
    }

    [HttpGet(Name = "Disconnect")]
    [Authorize("function.integration.ynab")]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    public async Task<ActionResult> Disconnect()
    {
        if (!await _connectService.IsConnected())
        {
            return BadRequest(new ApiResponse()
            {
                Errors = new List<AppApiError>
                {
                    new(ErrorType.BadRequest, "Not connected.")
                }
            });
        }

        await _connectService.Disconnect();
        return Ok(new ApiResponse());
    }
}