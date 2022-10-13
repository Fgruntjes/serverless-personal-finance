using App.Function.Banktransaction.Import.Service;
using App.Lib.Message;
using Microsoft.AspNetCore.Mvc;

namespace App.Function.Banktransaction.Import.Controller;

[ApiController]
[Route("/")]
public class AppController : Microsoft.AspNetCore.Mvc.Controller
{
    private readonly BankTransactionImportService _importService;

    public AppController(BankTransactionImportService importService)
    {
        _importService = importService;
    }

    [HttpPost(Name = "Import")]
    public async Task<ActionResult> Import([FromBody] BankTransactionImportMessage request)
    {
        try
        {
            await _importService.Import(request.Transactions);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }

        return Accepted();
    }
}