using GoFPatterns.AbtractFactory.Payments.Interfaces.Abstractions;

namespace GoFPatterns.AbtractFactory.Payments.PoolPays
{
    internal class PoolPayPaymentValidator : IPaymentValidator
    {
        public bool ValidatePayment(decimal amount)
        {
            Console.WriteLine($"PoolPay: validating payment {amount}.");
            return amount > 0; // Example validation logic
        }
    }
}
