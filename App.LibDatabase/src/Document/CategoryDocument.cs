using MongoDB.Bson;

namespace App.LibDatabase.Document;

public class CategoryDocument
{
    public ObjectId Id { get; set; }
    public string Name { get; set; }
    
    public CategoryDocument(string name)
    {
        Name = name;
    }
}