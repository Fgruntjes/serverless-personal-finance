using System.ComponentModel.DataAnnotations;
using App.Lib.Ynab;
using App.Lib.Dto.Frontend;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Function.Integration.Ynab.Controllers;

[ApiController]
[Route("/connect")]
public class ConnectController : ControllerBase
{
    private readonly IConnectService _connectService;

    public ConnectController(IConnectService connectService)
    {
        _connectService = connectService;
    }

    [HttpGet(Name = "Connect")]
    [Authorize("function.integration.ynab")]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    public async Task<ActionResult> Connect([Required] string returnUrl)
    {
        if (await _connectService.IsConnected())
        {
            return BadRequest(new ApiResponse<string>()
            {
                Errors = new List<AppApiError>
                {
                    new(ErrorType.BadRequest, "Already connected.")
                }
            });
        }

        var redirectUri = _connectService.GetRedirectUrl(returnUrl);
        return Ok(new ApiResponse<string> { Data = redirectUri });
    }
}