namespace GoFPatterns.Proxy.BankAccount
{
    // Real subject - contains only business logic. No security, no caching, no logging.
    public class BankAccount(decimal initialBalance) : IBankAccount
    {
        private decimal _balance = initialBalance;

        public static int BalanceRequests { get; private set; }

        public decimal GetBalance()
        {
            BalanceRequests++;
            Thread.Sleep(100); // Emulation of request to bank core system.
            return _balance;
        }

        public void Withdraw(decimal amount)
        {
            if (amount > _balance)
                throw new InvalidOperationException("Not enough money.");
            _balance -= amount;
        }
    }
}
