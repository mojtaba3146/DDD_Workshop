using AutoFixture.Xunit2;
using FluentAssertions;
using Services.Domain.Transaction;

namespace Services.Spec;

public class TransactionOrchestratorSpecs
{
    [Theory, AutoMoqData]
    public void Transfer_adds_the_balance_to_the_debit_account(
        [Frozen] Accounts __,
        [Frozen(Matching.ImplementedInterfaces)] TransferService _,
        TransactionOrchestrator sut,
        AccountOrchestrator accountOrchestrator,
        AccountQueries queries,
        string transactionId,
        decimal amount,
        DateTime now,
        string description
    )
    {
        amount = Math.Abs(amount);
        var transactionParties =
            new TransactionParties(
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString());

        accountOrchestrator.OpenAccount(transactionParties.CreditAccountId, amount + 20000);

        sut.DraftTransfer(transactionId,
            transactionParties,
            amount, now, description);

        sut.CommitTransfer(transactionId);

        queries.GetBalanceForAccount(transactionParties.DebitAccountId).Should()
            .BeEquivalentTo(new { Balance = amount });
    }


    [Theory, AutoMoqData]
    public void Transfer_subtracts_the_balance_from_the_credit_account(
        [Frozen] Accounts __,
        [Frozen(Matching.ImplementedInterfaces)] TransferService _,
        TransactionOrchestrator sut,
        AccountOrchestrator accountService,
        AccountQueries queries,
        string transactionId,
        decimal amount,
        DateTime now,
        string description,
        string debitAccountId
        )
    {
        amount = Math.Abs(amount);
        var creditAccount = Build.AnAccount.WithBalance(amount + 25000).Please();
        var transactionParties =
            new TransactionParties(
                creditAccount.Id,
                Guid.NewGuid().ToString());

        accountService.OpenAccount(creditAccount.Id, creditAccount.Balance.Value);

        sut.DraftTransfer(transactionId,
            transactionParties,
            amount, now, description);

        sut.CommitTransfer(transactionId);

        queries.GetBalanceForAccount(creditAccount.Id).Should()
            .BeEquivalentTo(new { Balance = 25000 });
    }

    [Theory, AutoMoqData]
    public void Drafts_a_new_transaction(
        [Frozen] Transactions _,
        TransactionOrchestrator sut,
        TransactionQueries queries,
        DateTime now,
        string description,
        decimal amount
    )
    {
        amount = Math.Abs(amount);
        var transactionParties =
            new TransactionParties(
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString());

        sut.DraftTransfer("transaction Id", transactionParties, amount, now, description);

        queries.AllDrafts().Should().Contain(new TransferDraftViewModel(
            transactionParties.CreditAccountId,
            transactionParties.DebitAccountId,
            amount,
            now
        ));

    }

}