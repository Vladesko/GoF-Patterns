namespace GoFPatterns.Proxy.BankAccount.Proxies
{
    // Logging proxy - scrive un registro di audit di tutte le operazioni.
    public class LoggingAccountProxy(IBankAccount account) : IBankAccount
    {
        public decimal GetBalance()
        {
            var balance = account.GetBalance();
            Console.WriteLine($"  [audit] richiesto il saldo: {balance:F2}");
            return balance;
        }

        public void Withdraw(decimal amount)
        {
            Console.WriteLine($"  [audit] richiesto prelievo di {amount:F2}");
            try
            {
                account.Withdraw(amount);
                Console.WriteLine("  [audit] prelievo riuscito");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  [audit] prelievo non riuscito: {ex.Message}");
                throw;
            }
        }
    }
}
