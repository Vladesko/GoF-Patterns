using GoFPatterns.AbtractFactory.Payments.Interfaces.Abstractions;

namespace GoFPatterns.AbtractFactory.Payments.Stopes
{
    internal class StropePaymentProcessor : IPaymentProcessor
    {
        public Guid ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Strope: Zahlung über {amount} wird verarbeitet.");
            return Guid.NewGuid();
        }
    }
}
