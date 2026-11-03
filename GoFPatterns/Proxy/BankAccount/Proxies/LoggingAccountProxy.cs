namespace GoFPatterns.Proxy.BankAccount.Proxies
{
    // Proxy de logging: escribe un registro de auditoría de todas las operaciones.
    public class LoggingAccountProxy(IBankAccount account) : IBankAccount
    {
        public decimal GetBalance()
        {
            var balance = account.GetBalance();
            Console.WriteLine($"  [auditoría] saldo solicitado: {balance:F2}");
            return balance;
        }

        public void Withdraw(decimal amount)
        {
            Console.WriteLine($"  [auditoría] retirada de {amount:F2} solicitada");
            try
            {
                account.Withdraw(amount);
                Console.WriteLine("  [auditoría] retirada realizada");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  [auditoría] la retirada falló: {ex.Message}");
                throw;
            }
        }
    }
}
