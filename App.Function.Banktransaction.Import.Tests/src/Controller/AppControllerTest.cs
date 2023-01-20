using System.Net;
using App.Lib.Message;
using App.Lib.Database;
using App.Lib.Database.Document;
using App.Lib.Dto.Backend;
using App.Lib.Tests;
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
        var values = new BankTransactionImportMessage(new BankTransaction[]
        {
            new(
                new DateTime(2022, 10, 4),
                "SomeStore",
                "NL83INGB0123123",
                "EUR",
                220
            )
        });
        var response = await _client.PostAsJsonAsync("/", values);
        response.Should().HaveStatusCode(HttpStatusCode.Accepted);

        await WaitForTransactions(values);
    }


    [Fact]
    public async void OnlyCreatesTransactionOncePerTransactionId()
    {
        var transactionsInsert = new BankTransactionImportMessage(new BankTransaction[]
        {
            new(
                new DateTime(2022, 10, 4),
                "SomeStore",
                "NL83INGB0123123",
                "EUR",
                220
            )
            {
                TransactionId = "asdfasdf",
            }
        });
        await _client.PostAsJsonAsync("/", transactionsInsert);
        await WaitForTransactions(transactionsInsert);

        var transactionsUpdate = new BankTransactionImportMessage(new BankTransaction[]
        {
            new(
                new DateTime(2022, 10, 4),
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
                "SomeStore",
                "NL83INGB0123123",
                "EUR",
                220
            )
            {
                TransactionId = "asdfasdf2",
            }
        });
        await _client.PostAsJsonAsync("/", transactionsUpdate);
        await WaitForTransactions(transactionsUpdate);
    }

    [Fact]
    public async void OnlyCreatesTransactionOncePerValues()
    {
        var transactionsInsert = new BankTransactionImportMessage(new BankTransaction[]
        {
            new(
                new DateTime(2022, 10, 4),
                "SomeStore",
                "NL83INGB0123123",
                "EUR",
                220
            )
        });
        await _client.PostAsJsonAsync("/", transactionsInsert);
        await WaitForTransactions(transactionsInsert);

        var transactionsUpdate = new BankTransactionImportMessage(new BankTransaction[]
        {
            new(
                new DateTime(2022, 10, 4),
                "SomeStore",
                "NL83INGB0123123",
                "EUR",
                220
            )
        });
        await _client.PostAsJsonAsync("/", transactionsUpdate);
        await WaitForTransactions(transactionsUpdate);
    }

    [Fact]
    public async void OnlyCreatesReferencesOnce()
    {
        var transactionsUpdate = new BankTransactionImportMessage(new BankTransaction[]
        {
            new(
                new DateTime(2022, 10, 4),
                "SomeStore",
                "NL83INGB0123123",
                "EUR",
                220
            ),
            new(
                new DateTime(2022, 10, 4),
                "SomeStore",
                "NL83INGB0123123",
                "EUR",
                200
            )
        });
        await _client.PostAsJsonAsync("/", transactionsUpdate);
        await WaitForTransactions(transactionsUpdate);

        var dbContext = _factory.Services.GetRequiredService<DatabaseContext>();
        (await dbContext.GetCollection<AccountDocument>().CountDocumentsAsync(d => true)).Should().Be(1);
        (await dbContext.GetCollection<PayeeDocument>().CountDocumentsAsync(d => true)).Should().Be(1);
        (await dbContext.GetCollection<CurrencyDocument>().CountDocumentsAsync(d => true)).Should().Be(1);
    }

    [Fact]
    public async void ThrowBadRequestOnZeroTransactions()
    {
        var transactionsUpdate = new BankTransactionImportMessage(Array.Empty<BankTransaction>());
        var response = await _client.PostAsJsonAsync("/", transactionsUpdate);
        response.Should().HaveClientError();
        (await response.Content.ReadAsStringAsync()).Should().Contain("at least 1 transaction");
        await WaitForTransactions(transactionsUpdate);
    }

    private async Task<IList<BankTransaction>> GetDatabaseTransactions()
    {
        var dbContext = _factory.Services.GetRequiredService<DatabaseContext>();
        var transactionCollection = dbContext.GetCollection<BankTransactionDocument>();
        var accountCollection = dbContext.GetCollection<AccountDocument>();
        var currencyCollection = dbContext.GetCollection<CurrencyDocument>();
        var payeeCollection = dbContext.GetCollection<PayeeDocument>();

        var list = new List<BankTransaction>();
        foreach (var transaction in await transactionCollection.AsQueryable().ToListAsync())
        {
            var account = await accountCollection.Find(d => d.Id.Equals(transaction.AccountId))
                .FirstOrDefaultAsync();
            var currency = await currencyCollection.Find(d => d.Id.Equals(transaction.CurrencyId))
                .FirstOrDefaultAsync();
            var payee = await payeeCollection.Find(d => d.Id.Equals(transaction.PayeeId))
                .FirstOrDefaultAsync();

            list.Add(new BankTransaction(
                transaction.Date,
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

    private async Task WaitForTransactions(BankTransactionImportMessage importMessage)
    {
        var test = async () =>
        {
            IList<BankTransaction> list;
            do
            {
                list = await GetDatabaseTransactions();
            } while (list.Count != importMessage.Transactions.Length);

            list.Should().BeEquivalentTo(importMessage.Transactions);
        };
        await test.Should().CompleteWithinAsync(TimeSpan.FromSeconds(5));
    }
}