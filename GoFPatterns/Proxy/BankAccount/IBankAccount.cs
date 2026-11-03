namespace GoFPatterns.Proxy.BankAccount
{
    // Subject - operations with bank account.
    public interface IBankAccount
    {
        decimal GetBalance();
        void Withdraw(decimal amount);
    }
}
