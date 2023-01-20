using App.Lib.Dto;
using App.Lib.Dto.Backend;

namespace App.Lib.Message;

public class BankTransactionImportMessage
{
    public BankTransaction[] Transactions { get; }

    public BankTransactionImportMessage(BankTransaction[] transactions)
    {
        Transactions = transactions;
    }
}