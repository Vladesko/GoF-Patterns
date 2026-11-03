using GoFPatterns.AbtractFactory.Payments.Interfaces.Abstractions;

namespace GoFPatterns.AbtractFactory.Payments.Stopes
{
    internal class StropePaymentValidator : IPaymentValidator
    {
        public bool ValidatePayment(decimal amount)
        {
            Console.WriteLine($"Strope: convalida del pagamento {amount}.");
            return amount > 0; // Esempio di logica di convalida
        }
    }
}
