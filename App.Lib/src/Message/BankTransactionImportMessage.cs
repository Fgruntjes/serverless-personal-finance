using App.Lib.Dto;

namespace App.Lib.Message;

public class BankTransactionImportMessage
{
    public IList<BankTransaction> Transactions { get; }

    public BankTransactionImportMessage(IList<BankTransaction> transactions)
    {
        Transactions = transactions;
    }
}