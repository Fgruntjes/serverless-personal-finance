using MongoDB.Bson;

namespace App.LibDatabase.Document;

public class CurrencyDocument
{
    public ObjectId Id { get; set; }
    public string CurrencyCode { get; set; }
}