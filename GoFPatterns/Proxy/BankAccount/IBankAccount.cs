namespace GoFPatterns.Proxy.BankAccount
{
    // Субъект - операции с банковским счётом.
    public interface IBankAccount
    {
        decimal GetBalance();
        void Withdraw(decimal amount);
    }
}
