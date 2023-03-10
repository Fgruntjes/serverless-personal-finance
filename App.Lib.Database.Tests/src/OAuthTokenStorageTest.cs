using App.Lib.Database.Document;
using MongoDB.Driver;

namespace App.Lib.Database.Tests;

public class OAuthTokenStorageTest : DatabaseTest
{
    private const string TestTokenName = "test";
    private const string TestAccessToken = "secret_access_token";
    private const string TestRefreshToken = "secret_refresh_token";

    private readonly Guid TestTenant;
    private readonly OAuthTokenStorage _tokenStorage;
    private readonly IMongoCollection<OAuthTokenDocument> _tokenCollection;
    private readonly DateTime _testTokenExpiresAt;

    public OAuthTokenStorageTest()
    {
        TestTenant = Guid.NewGuid();
        _tokenStorage = new OAuthTokenStorage(_databaseContext);
        _tokenCollection = _databaseContext.GetCollection<OAuthTokenDocument>();
        _testTokenExpiresAt = new DateTime(2022, 12, 20, 10, 1, 2).ToUniversalTime();

        SerializerRegistrationHandler.RegisterSerializer(DataProtectorProviderMock.Create().Object);
    }

    [Fact]
    public async void Get_New()
    {
        (await _tokenStorage.Get(TestTokenName, TestTenant))
            .Should()
            .BeEquivalentTo(new OAuthToken
            {
                Name = "test",
                Tenant = TestTenant,
                AccessToken = null,
                RefreshToken = null,
                ExpiresAt = null
            });
    }

    [Fact]
    public async void Get_Existing()
    {
        var databaseToken = await CreateDatabaseToken();

        var token = await _tokenStorage.Get(TestTokenName, TestTenant);
        token.Name.Should().Be(TestTokenName);
        token.ExpiresAt.Should().Be(databaseToken.ExpiresAt);
        ((string)token.AccessToken).Should().Be(TestAccessToken);
        ((string)token.RefreshToken).Should().Be(TestRefreshToken);
    }

    [Fact]
    public async void Store_New()
    {
        await _tokenStorage.Store(new OAuthToken
        {
            Name = TestTokenName,
            Tenant = TestTenant,
            AccessToken = EncryptedString.FromDecryptedValue(TestAccessToken),
            RefreshToken = EncryptedString.FromDecryptedValue(TestRefreshToken),
            ExpiresAt = _testTokenExpiresAt,
        });

        var token = await GetDatabaseToken();
        token.Name.Should().Be(TestTokenName);
        token.ExpiresAt.Should().Be(_testTokenExpiresAt);
        ((string)token.AccessToken).Should().Be(TestAccessToken);
        ((string)token.RefreshToken).Should().Be(TestRefreshToken);
    }

    [Fact]
    public async void Store_UpdateExisting()
    {
        await CreateDatabaseToken();

        var newExpiresAt = new DateTime(2023, 12, 20, 10, 1, 2).ToUniversalTime();
        await _tokenStorage.Store(new OAuthToken()
        {
            Name = TestTokenName,
            Tenant = TestTenant,
            AccessToken = EncryptedString.FromDecryptedValue("new_access_token"),
            RefreshToken = EncryptedString.FromDecryptedValue("new_refresh_token"),
            ExpiresAt = newExpiresAt,
        });

        var newToken = await GetDatabaseToken();
        newToken.ExpiresAt.Should().Be(newExpiresAt);
        ((string)newToken.AccessToken).Should().Be("new_access_token");
        ((string)newToken.RefreshToken).Should().Be("new_refresh_token");
    }

    [Fact]
    public async void Store_Delete()
    {
        await CreateDatabaseToken();

        await _tokenStorage.Store(new OAuthToken()
        {
            Name = TestTokenName,
            Tenant = TestTenant
        });


        // Allow some time to make sure mongodb has updated the document
        await Task.Delay(2000);
        var cursor = await _tokenCollection.FindAsync(t => t.Name == TestTokenName && t.Tenant == TestTenant);
        var tokenList = await cursor.ToListAsync();
        tokenList.Should().BeEmpty();
    }

    [Fact]
    public async void Delete()
    {
        await CreateDatabaseToken();

        await _tokenStorage.Delete(TestTokenName, TestTenant);

        // Allow some time to make sure mongodb has updated the document
        await Task.Delay(2000);
        var cursor = await _tokenCollection.FindAsync(t => t.Name == TestTokenName && t.Tenant == TestTenant);
        var tokenList = await cursor.ToListAsync();
        tokenList.Should().BeEmpty();
    }

    private async Task<OAuthTokenDocument> CreateDatabaseToken()
    {
        var token = new OAuthTokenDocument(TestTokenName, TestTenant)
        {
            ExpiresAt = new DateTime(2022, 12, 20, 10, 1, 2).ToUniversalTime(),
            AccessToken = EncryptedString.FromDecryptedValue(TestAccessToken),
            RefreshToken = EncryptedString.FromDecryptedValue(TestRefreshToken)
        };
        await _tokenCollection.InsertOneAsync(token);

        return await GetDatabaseToken();
    }

    private async Task<OAuthTokenDocument> GetDatabaseToken()
    {
        OAuthTokenDocument token = null;
        var test = async () =>
        {
            do
            {
                var cursor = await _tokenCollection.FindAsync(t => t.Name == TestTokenName);
                var tokenList = await cursor.ToListAsync();
                token = tokenList.First();
            } while (token == null);
        };
        await test.Should().CompleteWithinAsync(TimeSpan.FromSeconds(5));

        return token;
    }
}