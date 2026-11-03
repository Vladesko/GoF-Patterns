using GoFPatterns.Proxy.BankAccount.Proxies;

namespace GoFPatterns.Proxy.BankAccount
{
    public class DemonstrateBankAccount
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Заместитель: банковский счёт (защита, кеширование, логирование) ===\n");

            var realAccount = new BankAccount(1_000m);

            // Заместители можно комбинировать, потому что все они реализуют IBankAccount.
            IBankAccount ownerAccess = new LoggingAccountProxy(
                new ProtectionAccountProxy(
                    new CachingAccountProxy(realAccount), UserRole.Owner));

            var requestsBefore = BankAccount.BalanceRequests;
            Console.WriteLine("Владелец проверяет баланс 3 раза:");
            for (var i = 0; i < 3; i++)
            {
                ownerAccess.GetBalance();
            }
            Console.WriteLine($"  Реальных запросов к банку: {BankAccount.BalanceRequests - requestsBefore}\n");

            Console.WriteLine("Владелец снимает деньги:");
            ownerAccess.Withdraw(300m);
            ownerAccess.GetBalance();

            Console.WriteLine("\nГость пытается снять деньги:");
            IBankAccount guestAccess = new LoggingAccountProxy(new ProtectionAccountProxy(realAccount, UserRole.Guest));
            try
            {
                guestAccess.Withdraw(100m);
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("  Доступ запрещён, деньги в сохранности.");
            }
            guestAccess.GetBalance();
        }
    }
}
