using System.Net;
using System.Net.Http.Json;
using App.Lib.Dto;
using App.TestsLib;

namespace App.Function.Banktransaction.Import.Tests.Controller;

public class AppControllerTest : IntegrationTestFixture<Program>
{
    public AppControllerTest(TestApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async void PostAndWaitForDatabase()
    {
        var values = new BankTransaction[]
        {
            new()
            {
                Account = "Some Account",
                Category = "Clothing",
                Date = new DateTime(2022, 10, 4),
                Payee = "SomeStore",
                Value = 220,
                CurrencyCode = "EUR",
            }
        };
        using var response = await _client.PostAsJsonAsync("/", values);
        Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
    }
}