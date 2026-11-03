using GoFPatterns.AbtractFactory.Payments.Interfaces.Abstractions;

namespace GoFPatterns.AbtractFactory.Payments.Stopes
{
    internal class StropePaymentValidator : IPaymentValidator
    {
        public bool ValidatePayment(decimal amount)
        {
            Console.WriteLine($"Strope: Zahlung über {amount} wird geprüft.");
            return amount > 0; // Beispielhafte Validierungslogik
        }
    }
}
