namespace GoFPatterns.Proxy.BankAccount.Proxies
{
    // Логирующий заместитель - ведёт журнал аудита всех операций.
    public class LoggingAccountProxy(IBankAccount account) : IBankAccount
    {
        public decimal GetBalance()
        {
            var balance = account.GetBalance();
            Console.WriteLine($"  [аудит] запрошен баланс: {balance:F2}");
            return balance;
        }

        public void Withdraw(decimal amount)
        {
            Console.WriteLine($"  [аудит] запрошено снятие {amount:F2}");
            try
            {
                account.Withdraw(amount);
                Console.WriteLine("  [аудит] снятие выполнено");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  [аудит] снятие не удалось: {ex.Message}");
                throw;
            }
        }
    }
}
