using App.Lib.Dto;

namespace App.Lib.Message;

public class BankTransactionImportMessage
{
    public BankTransaction[] Transactions { get; }

    public BankTransactionImportMessage(BankTransaction[] transactions)
    {
        Transactions = transactions;
    }
}