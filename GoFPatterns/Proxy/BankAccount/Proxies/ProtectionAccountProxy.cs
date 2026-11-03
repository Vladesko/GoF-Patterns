namespace GoFPatterns.Proxy.BankAccount.Proxies
{
    // Protection proxy - checks access rights before delegating the call to the real object.
    public class ProtectionAccountProxy(IBankAccount account, UserRole role) : IBankAccount
    {
        public decimal GetBalance() => account.GetBalance();

        public void Withdraw(decimal amount)
        {
            if (role != UserRole.Owner)
                throw new UnauthorizedAccessException($"{role} can't withdraw money.");

            account.Withdraw(amount);
        }
    }
}
