namespace GoFPatterns.AbtractFactory.Payments.Interfaces.Abstractions
{
    public interface IRefundProcessor
    {
        void ProcessRefund(Guid transactionId);
    }
}
