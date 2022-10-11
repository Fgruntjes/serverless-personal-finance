using System.Net;
using System.Net.Http.Json;
using App.Lib.Dto;
using App.LibDatabase;
using App.LibDatabase.Document;
using App.TestsLib;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace App.Function.Banktransaction.Import.Tests.Controller;

public class AppControllerTest : IntegrationTestFixture<Program>
{
    public AppControllerTest(TestApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async void PostAndWaitForDatabase()
    {
        var values = new BankTransaction[]
        {
            new(
                new DateTime(2022, 10, 4),
                "Clothing",
                "SomeStore",
                "NL83INGB0123123",
                "EUR",
                220
            )
        };
        var response = await _client.PostAsJsonAsync("/", values);
        response.Should().HaveStatusCode(HttpStatusCode.Accepted);

        await WaitForTransactions(values);
    }


    [Fact]
    public async void OnlyCreatesTransactionOncePerTransactionId()
    {
        var transactionsInsert = new BankTransaction[]
        {
            new(
                new DateTime(2022, 10, 4),
                "Clothing",
                "SomeStore",
                "NL83INGB0123123",
                "EUR",
                220
            )
            {
                TransactionId = "asdfasdf",
            }
        };
        await _client.PostAsJsonAsync("/", transactionsInsert);
        await WaitForTransactions(transactionsInsert);

        var transactionsUpdate = new BankTransaction[]
        {
            new(
                new DateTime(2022, 10, 4),
                "Clothing",
                "SomeStore",
                "NL83INGB0123123",
                "EUR",
                220
            )
            {
                TransactionId = "asdfasdf",
            },
            new(
                new DateTime(2022, 10, 4),
                "Clothing",
                "SomeStore",
                "NL83INGB0123123",
                "EUR",
                220
            )
            {
                TransactionId = "asdfasdf2",
            }
        };
        await _client.PostAsJsonAsync("/", transactionsUpdate);
        await WaitForTransactions(transactionsUpdate);
    }

    [Fact]
    public async void OnlyCreatesTransactionOncePerValues()
    {
        var transactionsInsert = new BankTransaction[]
        {
            new(
                new DateTime(2022, 10, 4),
                "Clothing",
                "SomeStore",
                "NL83INGB0123123",
                "EUR",
                220
            )
        };
        await _client.PostAsJsonAsync("/", transactionsInsert);
        await WaitForTransactions(transactionsInsert);

        var transactionsUpdate = new BankTransaction[]
        {
            new(
                new DateTime(2022, 10, 4),
                "Working",
                "SomeStore",
                "NL83INGB0123123",
                "EUR",
                220
            )
        };
        await _client.PostAsJsonAsync("/", transactionsUpdate);
        await WaitForTransactions(transactionsUpdate);
    }

    [Fact]
    public async void OnlyCreatesReferencesOnce()
    {
        var transactionsUpdate = new BankTransaction[]
        {
            new(
                new DateTime(2022, 10, 4),
                "Working",
                "SomeStore",
                "NL83INGB0123123",
                "EUR",
                220
            ),
            new(
                new DateTime(2022, 10, 4),
                "Working",
                "SomeStore",
                "NL83INGB0123123",
                "EUR",
                200
            )
        };
        await _client.PostAsJsonAsync("/", transactionsUpdate);
        await WaitForTransactions(transactionsUpdate);

        var dbContext = _factory.Services.GetRequiredService<DbContext>();
        (await dbContext.GetCollection<AccountDocument>().CountDocumentsAsync(d => true)).Should().Be(1);
        (await dbContext.GetCollection<CategoryDocument>().CountDocumentsAsync(d => true)).Should().Be(1);
        (await dbContext.GetCollection<PayeeDocument>().CountDocumentsAsync(d => true)).Should().Be(1);
        (await dbContext.GetCollection<CurrencyDocument>().CountDocumentsAsync(d => true)).Should().Be(1);
    }

    private async Task<IList<BankTransaction>> GetDatabaseTransactions()
    {
        var dbContext = _factory.Services.GetRequiredService<DbContext>();
        var transactionCollection = dbContext.GetCollection<BankTransactionDocument>();
        var accountCollection = dbContext.GetCollection<AccountDocument>();
        var categoryCollection = dbContext.GetCollection<CategoryDocument>();
        var currencyCollection = dbContext.GetCollection<CurrencyDocument>();
        var payeeCollection = dbContext.GetCollection<PayeeDocument>();

        var list = new List<BankTransaction>();
        foreach (var transaction in await transactionCollection.AsQueryable().ToListAsync())
        {
            var category = await categoryCollection.Find(d => d.Id.Equals(transaction.CategoryId))
                .FirstOrDefaultAsync();
            var account = await accountCollection.Find(d => d.Id.Equals(transaction.AccountId))
                .FirstOrDefaultAsync();
            var currency = await currencyCollection.Find(d => d.Id.Equals(transaction.CurrencyId))
                .FirstOrDefaultAsync();
            var payee = await payeeCollection.Find(d => d.Id.Equals(transaction.PayeeId))
                .FirstOrDefaultAsync();

            list.Add(new BankTransaction(
                transaction.Date,
                category.Name,
                payee.Name,
                account.AccountNumber,
                currency.CurrencyCode,
                transaction.Amount
            )
            {
                PayeeDescription = transaction.PayeeDescription,
                TransactionId = transaction.TransactionId
            });
        }

        return list;
    }

    private async Task WaitForTransactions(BankTransaction[] values)
    {
        var test = async () =>
        {
            IList<BankTransaction> list;
            do
            {
                list = await GetDatabaseTransactions();
            } while (list.Count != values.Length);

            list.Should().BeEquivalentTo(values);
        };
        await test.Should().CompleteWithinAsync(TimeSpan.FromSeconds(5));
    }
}