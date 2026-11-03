using GoFPatterns.Proxy.BankAccount.Proxies;

namespace GoFPatterns.Proxy.BankAccount
{
    public class DemonstrateBankAccount
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Proxy: Bank account (protection, caching, logging) ===\n");

            var realAccount = new BankAccount(1_000m);

            // Proxies can be combined, because all of them implement IBankAccount.
            IBankAccount ownerAccess = new LoggingAccountProxy(
                new ProtectionAccountProxy(
                    new CachingAccountProxy(realAccount), UserRole.Owner));

            var requestsBefore = BankAccount.BalanceRequests;
            Console.WriteLine("Owner checks balance 3 times:");
            for (var i = 0; i < 3; i++)
            {
                ownerAccess.GetBalance();
            }
            Console.WriteLine($"  Real requests to bank: {BankAccount.BalanceRequests - requestsBefore}\n");

            Console.WriteLine("Owner withdraws money:");
            ownerAccess.Withdraw(300m);
            ownerAccess.GetBalance();

            Console.WriteLine("\nGuest tries to withdraw money:");
            IBankAccount guestAccess = new LoggingAccountProxy(new ProtectionAccountProxy(realAccount, UserRole.Guest));
            try
            {
                guestAccess.Withdraw(100m);
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("  Access denied, money is safe.");
            }
            guestAccess.GetBalance();
        }
    }
}
