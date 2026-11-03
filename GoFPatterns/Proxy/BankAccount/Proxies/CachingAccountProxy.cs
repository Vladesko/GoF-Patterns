namespace GoFPatterns.Proxy.BankAccount.Proxies
{
    // Proxy de caché: recuerda el resultado de una llamada costosa y lo invalida cuando cambian los datos.
    public class CachingAccountProxy(IBankAccount account) : IBankAccount
    {
        private decimal? _cachedBalance;

        public decimal GetBalance() => _cachedBalance ??= account.GetBalance();

        public void Withdraw(decimal amount)
        {
            account.Withdraw(amount);
            _cachedBalance = null; // El saldo ha cambiado: la caché ya no es válida.
        }
    }
}
