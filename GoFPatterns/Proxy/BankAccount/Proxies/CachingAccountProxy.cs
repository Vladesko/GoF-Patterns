namespace GoFPatterns.Proxy.BankAccount.Proxies
{
    // Кеширующий заместитель - запоминает результат дорогого вызова и сбрасывает его при изменении данных.
    public class CachingAccountProxy(IBankAccount account) : IBankAccount
    {
        private decimal? _cachedBalance;

        public decimal GetBalance() => _cachedBalance ??= account.GetBalance();

        public void Withdraw(decimal amount)
        {
            account.Withdraw(amount);
            _cachedBalance = null; // Баланс изменился - кеш больше не действителен.
        }
    }
}
