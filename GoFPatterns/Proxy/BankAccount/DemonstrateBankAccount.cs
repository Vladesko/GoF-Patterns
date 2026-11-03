using GoFPatterns.Proxy.BankAccount.Proxies;

namespace GoFPatterns.Proxy.BankAccount
{
    public class DemonstrateBankAccount
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Proxy: cuenta bancaria (protección, caché, logging) ===\n");

            var realAccount = new BankAccount(1_000m);

            // Los proxies se pueden combinar, porque todos implementan IBankAccount.
            IBankAccount ownerAccess = new LoggingAccountProxy(
                new ProtectionAccountProxy(
                    new CachingAccountProxy(realAccount), UserRole.Owner));

            var requestsBefore = BankAccount.BalanceRequests;
            Console.WriteLine("El titular consulta el saldo 3 veces:");
            for (var i = 0; i < 3; i++)
            {
                ownerAccess.GetBalance();
            }
            Console.WriteLine($"  Peticiones reales al banco: {BankAccount.BalanceRequests - requestsBefore}\n");

            Console.WriteLine("El titular retira dinero:");
            ownerAccess.Withdraw(300m);
            ownerAccess.GetBalance();

            Console.WriteLine("\nUn invitado intenta retirar dinero:");
            IBankAccount guestAccess = new LoggingAccountProxy(new ProtectionAccountProxy(realAccount, UserRole.Guest));
            try
            {
                guestAccess.Withdraw(100m);
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("  Acceso denegado, el dinero está a salvo.");
            }
            guestAccess.GetBalance();
        }
    }
}
