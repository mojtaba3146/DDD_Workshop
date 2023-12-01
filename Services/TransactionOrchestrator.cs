﻿using Services.Domain.Transaction;

namespace Services;

public class TransactionOrchestrator
{
    readonly Transactions transactions;
    readonly ITransferService transferService;
    public TransactionOrchestrator(Transactions transactions, ITransferService transferService)
    {
        this.transactions = transactions;
        this.transferService = transferService;
    }

    public void DraftTransfer(string transactionId, TransactionParties transactionParties, decimal amount, DateTime transactionDate, string description)
    {
        transactions.Add(Transaction.Draft(transactionId, transactionDate, description, transactionParties, amount));
    }

    public void CommitTransfer(
        string transactionId)
    {
        var draft = transactions.FindById(transactionId);

        if(draft is null) throw new InvalidOperationException($"No transaction drafts with the id: {transactionId}");
        
        draft.Commit(transferService);

        transactions.Update(draft);
    }
}
