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
        
    }
}