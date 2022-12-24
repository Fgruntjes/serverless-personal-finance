using App.Lib.Ynab;
using App.Lib.Dto.Frontend;
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
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status302Found)]
    public async Task<ActionResult> Connect()
    {
        if (await _connectService.IsConnected())
        {
            return BadRequest(new ApiResponse<bool>()
            {
                Errors = new List<ApiError>
                {
                    new(ErrorType.BadRequest, "Already connected.")
                }
            });
        }

        var redirectUri = _connectService.GetRedirectUrl();
        return Redirect(redirectUri);
    }
}