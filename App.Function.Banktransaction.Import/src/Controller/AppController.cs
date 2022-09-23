using App.Lib.Dto;
using Microsoft.AspNetCore.Mvc;

namespace App.Function.Banktransaction.Import.Controller;

[ApiController]
[Route("/")]
public class AppController : Microsoft.AspNetCore.Mvc.Controller
{
    [HttpPost(Name = "Import")]
    public async Task<ActionResult> Import([FromBody] BankTransaction[] transactions)
    {
        return Accepted();
    }
}