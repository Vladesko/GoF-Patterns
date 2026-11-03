using GoFPatterns.AbtractFactory.Payments.Interfaces.Abstractions;

namespace GoFPatterns.AbtractFactory.Payments.PoolPays
{
    internal class PoolPayRefundProcessor : IRefundProcessor
    {
        public void ProcessRefund(Guid transactionId)
        {
            Console.WriteLine($"PoolPay: elaborazione del rimborso per la transazione {transactionId}.");
        }
    }
}
