namespace GoFPatterns.Proxy.BankAccount.Proxies
{
    // Caching proxy - remembers result of expensive call and invalidates it when data changes.
    public class CachingAccountProxy(IBankAccount account) : IBankAccount
    {
        private decimal? _cachedBalance;

        public decimal GetBalance() => _cachedBalance ??= account.GetBalance();

        public void Withdraw(decimal amount)
        {
            account.Withdraw(amount);
            _cachedBalance = null; // Balance is changed - cache is not valid anymore.
        }
    }
}
