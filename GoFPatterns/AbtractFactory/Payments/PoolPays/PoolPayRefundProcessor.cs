using GoFPatterns.AbtractFactory.Payments.Interfaces.Abstractions;

namespace GoFPatterns.AbtractFactory.Payments.PoolPays
{
    internal class PoolPayRefundProcessor : IRefundProcessor
    {
        public void ProcessRefund(Guid transactionId)
        {
            Console.WriteLine($"PoolPay: Erstattung für Transaktion {transactionId} wird verarbeitet.");
        }
    }
}
