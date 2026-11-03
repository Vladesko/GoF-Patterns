namespace GoFPatterns.Proxy.BankAccount.Proxies
{
    // Protection proxy - verifica i diritti di accesso prima di delegare la chiamata all'oggetto reale.
    public class ProtectionAccountProxy(IBankAccount account, UserRole role) : IBankAccount
    {
        public decimal GetBalance() => account.GetBalance();

        public void Withdraw(decimal amount)
        {
            if (role != UserRole.Owner)
                throw new UnauthorizedAccessException($"{role} non può prelevare denaro.");

            account.Withdraw(amount);
        }
    }
}
