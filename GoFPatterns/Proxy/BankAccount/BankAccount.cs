namespace GoFPatterns.Proxy.BankAccount
{
    // Sujeto real: contiene solo la lógica de negocio. Sin seguridad, sin caché y sin logging.
    public class BankAccount(decimal initialBalance) : IBankAccount
    {
        private decimal _balance = initialBalance;

        public static int BalanceRequests { get; private set; }

        public decimal GetBalance()
        {
            BalanceRequests++;
            Thread.Sleep(100); // Emulación de una petición al sistema central del banco.
            return _balance;
        }

        public void Withdraw(decimal amount)
        {
            if (amount > _balance)
                throw new InvalidOperationException("No hay suficiente dinero.");
            _balance -= amount;
        }
    }
}
