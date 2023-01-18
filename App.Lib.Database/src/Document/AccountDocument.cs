using MongoDB.Bson;

namespace App.Lib.Database.Document;

public class AccountDocument
{
    public ObjectId Id { get; set; }
    public string AccountNumber { get; set; }

    public AccountDocument(string accountNumber)
    {
        AccountNumber = accountNumber;
    }
}