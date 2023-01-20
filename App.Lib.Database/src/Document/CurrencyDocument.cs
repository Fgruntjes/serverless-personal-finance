using MongoDB.Bson;

namespace App.Lib.Database.Document;

public class CurrencyDocument
{
    public ObjectId Id { get; set; }
    public string CurrencyCode { get; set; }

    public CurrencyDocument(string currencyCode)
    {
        CurrencyCode = currencyCode;
    }
}