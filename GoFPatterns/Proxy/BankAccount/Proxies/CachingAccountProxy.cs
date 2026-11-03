namespace GoFPatterns.Proxy.BankAccount.Proxies
{
    // Caching-Proxy – merkt sich das Ergebnis eines teuren Aufrufs und verwirft es, wenn sich die Daten ändern.
    public class CachingAccountProxy(IBankAccount account) : IBankAccount
    {
        private decimal? _cachedBalance;

        public decimal GetBalance() => _cachedBalance ??= account.GetBalance();

        public void Withdraw(decimal amount)
        {
            account.Withdraw(amount);
            _cachedBalance = null; // Der Kontostand hat sich geändert – der Cache ist nicht mehr gültig.
        }
    }
}
