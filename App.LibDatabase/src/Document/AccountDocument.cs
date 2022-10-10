using MongoDB.Bson;

namespace App.LibDatabase.Document;

public class AccountDocument
{
    public ObjectId Id { get; set; }
    public string AccountNumber { get; set; }
}