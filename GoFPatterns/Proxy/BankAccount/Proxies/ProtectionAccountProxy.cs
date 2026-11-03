namespace GoFPatterns.Proxy.BankAccount.Proxies
{
    // Защищающий заместитель - проверяет права доступа, прежде чем делегировать вызов реальному объекту.
    public class ProtectionAccountProxy(IBankAccount account, UserRole role) : IBankAccount
    {
        public decimal GetBalance() => account.GetBalance();

        public void Withdraw(decimal amount)
        {
            if (role != UserRole.Owner)
                throw new UnauthorizedAccessException($"Роль {role} не может снимать деньги.");

            account.Withdraw(amount);
        }
    }
}
