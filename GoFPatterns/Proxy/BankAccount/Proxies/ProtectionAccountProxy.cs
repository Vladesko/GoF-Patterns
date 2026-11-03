namespace GoFPatterns.Proxy.BankAccount.Proxies
{
    // Schutz-Proxy – prüft die Zugriffsrechte, bevor der Aufruf an das echte Objekt delegiert wird.
    public class ProtectionAccountProxy(IBankAccount account, UserRole role) : IBankAccount
    {
        public decimal GetBalance() => account.GetBalance();

        public void Withdraw(decimal amount)
        {
            if (role != UserRole.Owner)
                throw new UnauthorizedAccessException($"{role} darf kein Geld abheben.");

            account.Withdraw(amount);
        }
    }
}
