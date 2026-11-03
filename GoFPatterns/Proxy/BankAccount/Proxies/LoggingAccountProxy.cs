namespace GoFPatterns.Proxy.BankAccount.Proxies
{
    // Logging proxy - writes audit log of all operations.
    public class LoggingAccountProxy(IBankAccount account) : IBankAccount
    {
        public decimal GetBalance()
        {
            var balance = account.GetBalance();
            Console.WriteLine($"  [audit] balance requested: {balance:F2}");
            return balance;
        }

        public void Withdraw(decimal amount)
        {
            Console.WriteLine($"  [audit] withdraw {amount:F2} requested");
            try
            {
                account.Withdraw(amount);
                Console.WriteLine("  [audit] withdraw succeeded");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  [audit] withdraw failed: {ex.Message}");
                throw;
            }
        }
    }
}
