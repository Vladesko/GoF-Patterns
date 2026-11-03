namespace GoFPatterns.Proxy.BankAccount.Proxies
{
    // Caching proxy - ricorda il risultato di una chiamata costosa e lo invalida quando i dati cambiano.
    public class CachingAccountProxy(IBankAccount account) : IBankAccount
    {
        private decimal? _cachedBalance;

        public decimal GetBalance() => _cachedBalance ??= account.GetBalance();

        public void Withdraw(decimal amount)
        {
            account.Withdraw(amount);
            _cachedBalance = null; // Il saldo è cambiato - la cache non è più valida.
        }
    }
}
