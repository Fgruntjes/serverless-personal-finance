namespace App.LibDatabase;

public class DocumentMapFactory
{
    private readonly DbContext _dbContext;

    public DocumentMapFactory(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public DocumentMap<TKey, TDocument> Get<TKey, TDocument>() where TKey : notnull
    {
        return new DocumentMap<TKey, TDocument>(_dbContext.GetCollection<TDocument>());
    }
}