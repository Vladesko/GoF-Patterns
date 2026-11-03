namespace GoFPatterns.Proxy.BankAccount
{
    // Sujeto: operaciones con una cuenta bancaria.
    public interface IBankAccount
    {
        decimal GetBalance();
        void Withdraw(decimal amount);
    }
}
