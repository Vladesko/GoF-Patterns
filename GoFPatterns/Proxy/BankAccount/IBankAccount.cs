namespace GoFPatterns.Proxy.BankAccount
{
    // Subject - operazioni con il conto bancario.
    public interface IBankAccount
    {
        decimal GetBalance();
        void Withdraw(decimal amount);
    }
}
