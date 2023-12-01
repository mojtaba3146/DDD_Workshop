namespace Services.Domain.Transaction
{
    public class TransactionParties
    {
        public TransactionParties(
            string creditAccountId,
            string debitAccountId)
        {
            CreditAccountId = creditAccountId;
            DebitAccountId = debitAccountId;
        }

        public string CreditAccountId { get; private set; }
        public string DebitAccountId { get; private set; }
    }
}
