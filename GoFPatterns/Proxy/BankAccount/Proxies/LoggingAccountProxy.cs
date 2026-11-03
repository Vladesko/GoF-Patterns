namespace GoFPatterns.Proxy.BankAccount.Proxies
{
    // Logging-Proxy – schreibt ein Audit-Protokoll aller Operationen.
    public class LoggingAccountProxy(IBankAccount account) : IBankAccount
    {
        public decimal GetBalance()
        {
            var balance = account.GetBalance();
            Console.WriteLine($"  [audit] Kontostand abgefragt: {balance:F2}");
            return balance;
        }

        public void Withdraw(decimal amount)
        {
            Console.WriteLine($"  [audit] Abhebung von {amount:F2} angefordert");
            try
            {
                account.Withdraw(amount);
                Console.WriteLine("  [audit] Abhebung erfolgreich");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  [audit] Abhebung fehlgeschlagen: {ex.Message}");
                throw;
            }
        }
    }
}
