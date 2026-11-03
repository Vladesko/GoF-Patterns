namespace GoFPatterns.Proxy.BankAccount
{
    // Echtes Subjekt – enthält nur Geschäftslogik. Keine Sicherheit, kein Caching, kein Logging.
    public class BankAccount(decimal initialBalance) : IBankAccount
    {
        private decimal _balance = initialBalance;

        public static int BalanceRequests { get; private set; }

        public decimal GetBalance()
        {
            BalanceRequests++;
            Thread.Sleep(100); // Emulation einer Anfrage an das Kernbankensystem.
            return _balance;
        }

        public void Withdraw(decimal amount)
        {
            if (amount > _balance)
                throw new InvalidOperationException("Nicht genug Geld.");
            _balance -= amount;
        }
    }
}
