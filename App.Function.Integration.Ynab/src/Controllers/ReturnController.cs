using System.ComponentModel.DataAnnotations;
using App.Lib.Dto.Frontend;
using App.Lib.Ynab;
using App.Lib.Ynab.Exception;
using App.Lib.Ynab.Rest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Function.Integration.Ynab.Controllers;

[ApiController]
[Route("/return")]
public class ReturnController : ControllerBase
{
    private readonly IConnectService _connectService;
    private readonly IApiClient _client;

    public ReturnController(IConnectService connectService, IApiClient client)
    {
        _connectService = connectService;
        _client = client;
    }

    [HttpGet(Name = "Return")]
    [Authorize("function.integration.ynab")]
    [ProducesResponseType(typeof(ApiResponse<IntegrationStatus>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<IntegrationStatus>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Return([Required] string code, [Required] string returnUrl)
    {
        try
        {
            await _connectService.ProcessReturn(code, returnUrl);
            var connectedUser = await _client.GetUser();
            return Ok(new ApiResponse<IntegrationStatus>(
                new IntegrationStatus(true, connectedUser.Data.User.Id)
            ));
        }
        catch (TokenException httpException)
        {
            return BadRequest(new ApiResponse<IntegrationStatus>(new IntegrationStatus(false))
            {
                Errors = new[] { new AppApiError(ErrorType.Integration, httpException.Message) }
            });
        }
    }
}