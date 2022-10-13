using App.Job.Import.Ynab.Message;
using App.Job.Import.Ynab.Service;
using Microsoft.AspNetCore.Mvc;

namespace App.Job.Import.Ynab.Controller;

[ApiController]
[Route("/")]
public class AppController : Microsoft.AspNetCore.Mvc.Controller
{
    private readonly YnabImportService _importService;

    public AppController(YnabImportService importService)
    {
        _importService = importService;
    }

    [HttpPost(Name = "Import")]
    public async Task<ActionResult> Import([FromBody] YnabImportMessage message)
    {
        try
        {
            await _importService.Import();
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }

        return Accepted();
    }
}