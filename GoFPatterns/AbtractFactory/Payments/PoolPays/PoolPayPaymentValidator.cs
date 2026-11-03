using GoFPatterns.AbtractFactory.Payments.Interfaces.Abstractions;

namespace GoFPatterns.AbtractFactory.Payments.PoolPays
{
    internal class PoolPayPaymentValidator : IPaymentValidator
    {
        public bool ValidatePayment(decimal amount)
        {
            Console.WriteLine($"PoolPay: Zahlung über {amount} wird geprüft.");
            return amount > 0; // Beispielhafte Validierungslogik
        }
    }
}
