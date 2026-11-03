using GoFPatterns.AbtractFactory.Payments.Interfaces.Abstractions;

namespace GoFPatterns.AbtractFactory.Payments.PoolPays
{
    internal class PoolPayPaymentValidator : IPaymentValidator
    {
        public bool ValidatePayment(decimal amount)
        {
            Console.WriteLine($"PoolPay: convalida del pagamento {amount}.");
            return amount > 0; // Esempio di logica di convalida
        }
    }
}
