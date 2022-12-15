using System.ComponentModel.DataAnnotations;
using App.Lib.Dto.Frontend;
using App.Lib.Ynab;
using App.Lib.Ynab.Exception;
using App.Lib.Ynab.Rest;
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
    [ProducesResponseType(typeof(Lib.Dto.Frontend.ApiResponse<IntegrationStatus>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Lib.Dto.Frontend.ApiResponse<IntegrationStatus>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Return([Required] string code)
    {
        try
        {
            await _connectService.ProcessReturn(code);
            var connectedUser = await _client.GetUser();
            return Ok(new Lib.Dto.Frontend.ApiResponse<IntegrationStatus>(
                new IntegrationStatus(true, connectedUser.Data.Id)
            ));
        }
        catch (TokenException httpException)
        {
            return BadRequest(new Lib.Dto.Frontend.ApiResponse<IntegrationStatus>(new IntegrationStatus(false))
            {
                Errors = new[] { new ApiError(ErrorType.Integration, httpException.Message) }
            });
        }
    }
}