using App.Lib.Dto;
using App.Lib.Database;
using App.Lib.Database.Document;
using App.Lib.Dto.Backend;
using MongoDB.Driver;

namespace App.Function.Banktransaction.Import.Service;

public class BankTransactionImportService
{
    private readonly DatabaseContext _databaseContext;
    private readonly DocumentMapFactory _documentMapFactory;

    public BankTransactionImportService(DatabaseContext databaseContext, DocumentMapFactory documentMapFactory)
    {
        _databaseContext = databaseContext;
        _documentMapFactory = documentMapFactory;
    }

    public async Task Import(BankTransaction[] transactions)
    {
        if (transactions.Length == 0)
            throw new ArgumentException("Need to import at least 1 transaction.", nameof(transactions));

        var accountMap = await GetAccountMap(transactions);
        var categoryMap = await GetCategoryMap(transactions);
        var payeeMap = await GetPayeeMap(transactions);
        var currencyMap = await GetCurrencyMap(transactions);

        var documents = transactions.AsQueryable()
            .Select(t => new BankTransactionDocument
            {
                TransactionId = t.TransactionId,
                AccountId = accountMap[t.AccountNumber].Id,
                CategoryId = !string.IsNullOrWhiteSpace(t.Category) ? categoryMap[t.Category].Id : null,
                Date = t.Date,
                PayeeId = payeeMap[t.PayeeName].Id,
                PayeeDescription = t.PayeeDescription,
                Amount = t.Amount,
                CurrencyId = currencyMap[t.CurrencyCode].Id,
            });

        var operations = new List<WriteModel<BankTransactionDocument>>();
        foreach (var doc in documents)
        {
            FilterDefinition<BankTransactionDocument> filter;
            UpdateDefinition<BankTransactionDocument> update;
            if (string.IsNullOrWhiteSpace(doc.TransactionId))
            {
                filter = new ExpressionFilterDefinition<BankTransactionDocument>(
                    t =>
                        t.Date == doc.Date &&
                        t.Amount == doc.Amount &&
                        t.AccountId == doc.AccountId &&
                        t.PayeeId == doc.PayeeId &&
                        t.PayeeDescription == doc.PayeeDescription &&
                        t.CurrencyId == doc.CurrencyId
                );

                update = Builders<BankTransactionDocument>.Update
                    .Set(t => t.CategoryId, doc.CategoryId);
            }
            else
            {
                filter = new ExpressionFilterDefinition<BankTransactionDocument>(
                    t => t.TransactionId == doc.TransactionId
                );

                update = Builders<BankTransactionDocument>.Update
                    .Set(t => t.Date, doc.Date)
                    .Set(t => t.Amount, doc.Amount)
                    .Set(t => t.AccountId, doc.AccountId)
                    .Set(t => t.CategoryId, doc.CategoryId)
                    .Set(t => t.PayeeId, doc.PayeeId)
                    .Set(t => t.PayeeDescription, doc.PayeeDescription)
                    .Set(t => t.CurrencyId, doc.CurrencyId);
            }

            var operation = new UpdateOneModel<BankTransactionDocument>(filter, update)
            {
                IsUpsert = true
            };
            operations.Add(operation);
        }

        await _databaseContext.GetCollection<BankTransactionDocument>()
            .BulkWriteAsync(operations);
    }

    private async Task<Dictionary<string, AccountDocument>> GetAccountMap(IEnumerable<BankTransaction> transactions)
    {
        var searchValues = transactions
            .AsQueryable()
            .Select(t => t.AccountNumber)
            .ToArray();

        return await _documentMapFactory.Get<string, AccountDocument>()
            .Load(d => searchValues.Contains(d.AccountNumber), d => d.AccountNumber)
            .Fill(searchValues, v => new AccountDocument(v));
    }

    private async Task<Dictionary<string, CategoryDocument>> GetCategoryMap(IEnumerable<BankTransaction> transactions)
    {
        string[] searchValues = transactions
            .AsQueryable()
            .Select(t => t.Category)
            .Where(v => !string.IsNullOrWhiteSpace(v))
            .ToArray()!;

        return await _documentMapFactory.Get<string, CategoryDocument>()
            .Load(d => searchValues.Contains(d.Name), d => d.Name)
            .Fill(searchValues, v => new CategoryDocument(v));
    }

    private async Task<Dictionary<string, PayeeDocument>> GetPayeeMap(IEnumerable<BankTransaction> transactions)
    {
        var searchValues = transactions
            .AsQueryable()
            .Select(t => t.PayeeName)
            .ToArray();

        return await _documentMapFactory.Get<string, PayeeDocument>()
            .Load(d => searchValues.Contains(d.Name), d => d.Name)
            .Fill(searchValues, v => new PayeeDocument(v));
    }

    private async Task<Dictionary<string, CurrencyDocument>> GetCurrencyMap(IEnumerable<BankTransaction> transactions)
    {
        var searchValues = transactions
            .AsQueryable()
            .Select(t => t.CurrencyCode)
            .ToArray();

        return await _documentMapFactory.Get<string, CurrencyDocument>()
            .Load(d => searchValues.Contains(d.CurrencyCode), d => d.CurrencyCode)
            .Fill(searchValues, v => new CurrencyDocument(v));
    }
}