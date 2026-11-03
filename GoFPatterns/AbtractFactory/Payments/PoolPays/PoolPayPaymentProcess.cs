using GoFPatterns.AbtractFactory.Payments.Interfaces.Abstractions;

namespace GoFPatterns.AbtractFactory.Payments.PoolPays
{
    internal class PoolPayPaymentProcess : IPaymentProcessor
    {
        public Guid ProcessPayment(decimal amount)
        {
            Console.WriteLine($"PoolPay: elaborazione del pagamento {amount}.");
            return Guid.NewGuid();
        }
    }
}
