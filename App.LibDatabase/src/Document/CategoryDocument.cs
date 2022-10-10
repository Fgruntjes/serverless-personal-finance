using MongoDB.Bson;

namespace App.LibDatabase.Document;

public class CategoryDocument
{
    public ObjectId Id { get; set; }
    public string Name { get; set; }
}