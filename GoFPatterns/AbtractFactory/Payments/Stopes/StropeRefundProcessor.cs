using GoFPatterns.AbtractFactory.Payments.Interfaces.Abstractions;

namespace GoFPatterns.AbtractFactory.Payments.Stopes
{
    internal class StropeRefundProcessor : IRefundProcessor
    {
        public void ProcessRefund(Guid transactionId)
        {
            Console.WriteLine($"Strope: processing refund for transaction {transactionId}.");
        }
    }
}
