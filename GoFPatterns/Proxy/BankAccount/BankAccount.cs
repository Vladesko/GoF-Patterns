namespace GoFPatterns.Proxy.BankAccount
{
    // Real subject - contiene solo la logica di business. Nessuna sicurezza, nessuna cache, nessun logging.
    public class BankAccount(decimal initialBalance) : IBankAccount
    {
        private decimal _balance = initialBalance;

        public static int BalanceRequests { get; private set; }

        public decimal GetBalance()
        {
            BalanceRequests++;
            Thread.Sleep(100); // Emulazione di una richiesta al sistema centrale della banca.
            return _balance;
        }

        public void Withdraw(decimal amount)
        {
            if (amount > _balance)
                throw new InvalidOperationException("Fondi insufficienti.");
            _balance -= amount;
        }
    }
}
