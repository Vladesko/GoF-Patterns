using GoFPatterns.Proxy.BankAccount.Proxies;

namespace GoFPatterns.Proxy.BankAccount
{
    public class DemonstrateBankAccount
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Proxy: Conto bancario (protezione, cache, logging) ===\n");

            var realAccount = new BankAccount(1_000m);

            // I proxy possono essere combinati, perché implementano tutti IBankAccount.
            IBankAccount ownerAccess = new LoggingAccountProxy(
                new ProtectionAccountProxy(
                    new CachingAccountProxy(realAccount), UserRole.Owner));

            var requestsBefore = BankAccount.BalanceRequests;
            Console.WriteLine("Il titolare controlla il saldo 3 volte:");
            for (var i = 0; i < 3; i++)
            {
                ownerAccess.GetBalance();
            }
            Console.WriteLine($"  Richieste reali alla banca: {BankAccount.BalanceRequests - requestsBefore}\n");

            Console.WriteLine("Il titolare preleva denaro:");
            ownerAccess.Withdraw(300m);
            ownerAccess.GetBalance();

            Console.WriteLine("\nUn ospite prova a prelevare denaro:");
            IBankAccount guestAccess = new LoggingAccountProxy(new ProtectionAccountProxy(realAccount, UserRole.Guest));
            try
            {
                guestAccess.Withdraw(100m);
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("  Accesso negato, il denaro è al sicuro.");
            }
            guestAccess.GetBalance();
        }
    }
}
