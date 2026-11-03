using GoFPatterns.AbtractFactory.Payments.Interfaces.Abstractions;

namespace GoFPatterns.AbtractFactory.Payments.Stopes
{
    internal class StropePaymentProcessor : IPaymentProcessor
    {
        public Guid ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Strope: elaborazione del pagamento {amount}.");
            return Guid.NewGuid();
        }
    }
}
