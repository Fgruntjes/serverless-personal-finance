using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace App.Lib.Dto.Backend;

public class BankTransaction
{
    [Description("If none provided id will be generated from all fields")]
    public string? TransactionId { get; set; }

    [BindRequired]
    public DateTime Date { get; set; }

    public string? Category { get; set; }

    [Description("Owner name of the other account")]
    [BindRequired]
    public string PayeeName { get; set; }

    [Description("Payment description provided by the payee")]
    public string? PayeeDescription { get; set; }

    [Description("Account number owned by user receiving or sending transaction")]
    [BindRequired]
    public string AccountNumber { get; set; }

    [Description("ISO 4217:2015 (ISO-3) currency code")]
    [BindRequired]
    public string CurrencyCode { get; set; }

    [BindRequired]
    public Decimal Amount { get; set; }

    public BankTransaction(DateTime date, string payeeName, string accountNumber, string currencyCode, decimal amount)
    {
        Date = date;
        PayeeName = payeeName;
        AccountNumber = accountNumber;
        CurrencyCode = currencyCode;
        Amount = amount;
    }
}