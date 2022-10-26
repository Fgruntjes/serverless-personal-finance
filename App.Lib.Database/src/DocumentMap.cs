using System.Linq.Expressions;
using MongoDB.Driver;

namespace App.Lib.Database;

public class DocumentMap<TKey, TDocument> : Dictionary<TKey, TDocument> where TKey : notnull
{
    private readonly IMongoCollection<TDocument> _collection;

    public DocumentMap(IMongoCollection<TDocument> collection)
    {
        _collection = collection;
    }

    public DocumentMap<TKey, TDocument> Load(
        Expression<Func<TDocument, bool>> documentFilter,
        Func<TDocument, TKey> keySelector
    )
    {
        var existing = _collection.AsQueryable().Where(documentFilter);
        foreach (var document in existing)
        {
            Add(keySelector(document), document);
        }

        return this;
    }

    public async Task<DocumentMap<TKey, TDocument>> Fill(
        TKey[] keys,
        Func<TKey, TDocument> newDocumentCreator
    )
    {
        // Add values that do not exist
        var newDocuments = new List<TDocument>();
        foreach (var key in keys)
        {
            if (ContainsKey(key))
                continue;

            var newDocument = newDocumentCreator(key);
            Add(key, newDocument);
            newDocuments.Add(newDocument);
        }

        if (newDocuments.Count > 0)
            await _collection.InsertManyAsync(newDocuments);

        return this;
    }
}