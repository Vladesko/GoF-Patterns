using GoFPatterns.Proxy.BankAccount.Proxies;

namespace GoFPatterns.Proxy.BankAccount
{
    public class DemonstrateBankAccount
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Proxy: Bankkonto (Schutz, Caching, Logging) ===\n");

            var realAccount = new BankAccount(1_000m);

            // Proxys lassen sich kombinieren, weil sie alle IBankAccount implementieren.
            IBankAccount ownerAccess = new LoggingAccountProxy(
                new ProtectionAccountProxy(
                    new CachingAccountProxy(realAccount), UserRole.Owner));

            var requestsBefore = BankAccount.BalanceRequests;
            Console.WriteLine("Der Inhaber prüft den Kontostand 3-mal:");
            for (var i = 0; i < 3; i++)
            {
                ownerAccess.GetBalance();
            }
            Console.WriteLine($"  Echte Anfragen an die Bank: {BankAccount.BalanceRequests - requestsBefore}\n");

            Console.WriteLine("Der Inhaber hebt Geld ab:");
            ownerAccess.Withdraw(300m);
            ownerAccess.GetBalance();

            Console.WriteLine("\nEin Gast versucht, Geld abzuheben:");
            IBankAccount guestAccess = new LoggingAccountProxy(new ProtectionAccountProxy(realAccount, UserRole.Guest));
            try
            {
                guestAccess.Withdraw(100m);
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("  Zugriff verweigert, das Geld ist sicher.");
            }
            guestAccess.GetBalance();
        }
    }
}
