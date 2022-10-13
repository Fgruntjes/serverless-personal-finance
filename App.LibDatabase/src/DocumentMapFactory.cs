namespace App.LibDatabase;

public class DocumentMapFactory
{
    private readonly DatabaseContext _databaseContext;

    public DocumentMapFactory(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public DocumentMap<TKey, TDocument> Get<TKey, TDocument>() where TKey : notnull
    {
        return new DocumentMap<TKey, TDocument>(_databaseContext.GetCollection<TDocument>());
    }
}