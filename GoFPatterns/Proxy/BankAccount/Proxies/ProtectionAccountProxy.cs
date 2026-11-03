namespace GoFPatterns.Proxy.BankAccount.Proxies
{
    // Proxy de protección: comprueba los derechos de acceso antes de delegar la llamada en el objeto real.
    public class ProtectionAccountProxy(IBankAccount account, UserRole role) : IBankAccount
    {
        public decimal GetBalance() => account.GetBalance();

        public void Withdraw(decimal amount)
        {
            if (role != UserRole.Owner)
                throw new UnauthorizedAccessException($"{role} no puede retirar dinero.");

            account.Withdraw(amount);
        }
    }
}
