using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace App.LibDatabase.Document;

public class BankTransactionDocument
{
    public ObjectId Id { get; set; }
    [BsonDateTimeOptions(DateOnly = true)]
    public DateTime Date { get; set; }
    public ObjectId CategoryId { get; set; }
    public ObjectId AccountId { get; set; }
    public ObjectId CurrencyId { get; set; }
    public ObjectId PayeeId { get; set; }
    public string? PayeeDescription { get; set; }
    public Decimal Amount { get; set; }
    public string? TransactionId { get; set; }
}