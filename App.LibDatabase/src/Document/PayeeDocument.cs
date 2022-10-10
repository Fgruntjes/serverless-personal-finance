using MongoDB.Bson;

namespace App.LibDatabase.Document;

public class PayeeDocument
{
    public ObjectId Id { get; set; }
    public string Name { get; set; }
    public string AccountNumber { get; set; }
}