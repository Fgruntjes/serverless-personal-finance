using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace App.Lib.Dto;

public class BankTransaction
{
    [Description("If none provided id will be generated from all fields")]
    public string? TransactionId { get; set; }

    [BindRequired]
    public DateTime Date { get; set; }

    [BindRequired]
    public string Category { get; set; }

    [BindRequired]
    public string Payee { get; set; }

    [BindRequired]
    public string Account { get; set; }

    [BindRequired]
    [Description("ISO 4217:2015 (ISO-3) currency code")]
    public string CurrencyCode { get; set; }

    [BindRequired]
    public Decimal Value { get; set; }
}