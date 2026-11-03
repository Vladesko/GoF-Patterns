using GoFPatterns.AbtractFactory.Payments.Interfaces.Abstractions;

namespace GoFPatterns.AbtractFactory.Payments.Stopes
{
    internal class StropeRefundProcessor : IRefundProcessor
    {
        public void ProcessRefund(Guid transactionId)
        {
            Console.WriteLine($"Strope: procesando el reembolso de la transacción {transactionId}.");
        }
    }
}
