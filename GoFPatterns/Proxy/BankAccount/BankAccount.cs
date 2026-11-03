namespace GoFPatterns.Proxy.BankAccount
{
    // Реальный субъект - содержит только бизнес-логику. Без защиты, кеширования и логирования.
    public class BankAccount(decimal initialBalance) : IBankAccount
    {
        private decimal _balance = initialBalance;

        public static int BalanceRequests { get; private set; }

        public decimal GetBalance()
        {
            BalanceRequests++;
            Thread.Sleep(100); // Эмуляция запроса к ядру банковской системы.
            return _balance;
        }

        public void Withdraw(decimal amount)
        {
            if (amount > _balance)
                throw new InvalidOperationException("Недостаточно средств.");
            _balance -= amount;
        }
    }
}
