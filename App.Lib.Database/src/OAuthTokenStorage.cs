using App.Lib.Database.Document;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace App.Lib.Database;

public class OAuthTokenStorage : IOAuthTokenStorage
{
    private readonly DatabaseContext _dbContext;

    public OAuthTokenStorage(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IOAuthToken> Get(string name, Guid tenant)
    {
        var tokenCollection = _dbContext.GetCollection<OAuthTokenDocument>();
        var token = await tokenCollection
            .AsQueryable()
            .Where(d => d.Name == name && d.Tenant == tenant)
            .FirstOrDefaultAsync();

        if (token != null)
        {
            return token;
        }

        // Insert new token but be sure not to override any already existing
        token = new OAuthTokenDocument(name, tenant);
        await tokenCollection.UpdateOneAsync(
            filter => filter.Name == name && filter.Tenant == tenant,
            Builders<OAuthTokenDocument>.Update
                .Set(field => field.Name, name)
                .Set(field => field.Tenant, tenant),
            new UpdateOptions { IsUpsert = true }
        );

        return token;
    }

    public async Task Store(IOAuthToken token)
    {
        if (string.IsNullOrEmpty(token.AccessToken))
        {
            await Delete(token.Name, token.Tenant);
            return;
        }

        await _dbContext.GetCollection<OAuthTokenDocument>().UpdateOneAsync(
            filter => filter.Name == token.Name && filter.Tenant == token.Tenant,
            Builders<OAuthTokenDocument>.Update
                .Set(field => field.Name, token.Name)
                .Set(field => field.Tenant, token.Tenant)
                .Set(field => field.AccessToken, token.AccessToken)
                .Set(field => field.RefreshToken, token.RefreshToken)
                .Set(field => field.ExpiresAt, token.ExpiresAt),
            new UpdateOptions { IsUpsert = true }
        );
    }

    public async Task Delete(string tokenName, Guid tenant)
    {
        await _dbContext.GetCollection<OAuthTokenDocument>()
            .DeleteOneAsync(filter => filter.Name == tokenName && filter.Tenant == tenant);
    }
}