using App.Lib.Database.Document;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace App.Lib.Database;

public class OAuthTokenStorage
{
    private readonly DatabaseContext _dbContext;

    public OAuthTokenStorage(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IOAuthToken> Get(string name)
    {
        var tokenCollection = _dbContext.GetCollection<OAuthTokenDocument>();
        var token = await tokenCollection
            .AsQueryable()
            .Where(d => d.Name == name)
            .FirstOrDefaultAsync();

        if (token != null)
        {
            return token;
        }

        // Insert new token but be sure not to override any already existing
        token = new OAuthTokenDocument(name);
        await tokenCollection.UpdateOneAsync(
            filter => filter.Name == name,
            Builders<OAuthTokenDocument>.Update.Set(field => field.Name, name),
            new UpdateOptions { IsUpsert = true }
        );

        return token;
    }

    public async Task Store(IOAuthToken token)
    {
        if (string.IsNullOrEmpty(token.AccessToken))
        {
            await _dbContext.GetCollection<OAuthTokenDocument>().DeleteOneAsync(filter => filter.Name == token.Name);
            return;
        }

        await _dbContext.GetCollection<OAuthTokenDocument>().UpdateOneAsync(
            filter => filter.Name == token.Name,
            Builders<OAuthTokenDocument>.Update
                .Set(field => field.Name, token.Name)
                .Set(field => field.AccessToken, token.AccessToken)
                .Set(field => field.RefreshToken, token.RefreshToken)
                .Set(field => field.ExpiresAt, token.ExpiresAt),
            new UpdateOptions { IsUpsert = true }
        );
    }
}