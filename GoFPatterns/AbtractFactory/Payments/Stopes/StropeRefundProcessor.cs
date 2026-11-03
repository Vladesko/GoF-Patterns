using GoFPatterns.AbtractFactory.Payments.Interfaces.Abstractions;

namespace GoFPatterns.AbtractFactory.Payments.Stopes
{
    internal class StropeRefundProcessor : IRefundProcessor
    {
        public void ProcessRefund(Guid transactionId)
        {
            Console.WriteLine($"Strope: Erstattung für Transaktion {transactionId} wird verarbeitet.");
        }
    }
}
