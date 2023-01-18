using MongoDB.Bson;

namespace App.Lib.Database.Document;

public class PayeeDocument
{
    public ObjectId Id { get; set; }
    public string Name { get; set; }
    public string? AccountNumber { get; set; }

    public PayeeDocument(string name)
    {
        Name = name;
    }
}