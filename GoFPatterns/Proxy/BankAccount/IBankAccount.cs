namespace GoFPatterns.Proxy.BankAccount
{
    // Subjekt – Operationen mit dem Bankkonto.
    public interface IBankAccount
    {
        decimal GetBalance();
        void Withdraw(decimal amount);
    }
}
