using Xunit;

namespace App.TestsLib;

public class IntegrationTestFixture<TEntryPoint> : IClassFixture<TestApplicationFactory<TEntryPoint>> where TEntryPoint : class
{
    protected readonly TestApplicationFactory<TEntryPoint> _factory;
    protected readonly HttpClient _client;

    public IntegrationTestFixture(TestApplicationFactory<TEntryPoint> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }
}