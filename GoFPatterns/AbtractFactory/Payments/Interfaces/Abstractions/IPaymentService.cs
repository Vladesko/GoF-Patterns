namespace GoFPatterns.AbtractFactory.Payments.Interfaces.Abstractions
{
    /// <summary>
    /// The simple payment service
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>
        /// Processes a payment of the specified amount.
        /// </summary>
        /// <param name="amount">The amount to pay.</param>
        Guid Pay(decimal amount);

        /// <summary>
        /// Refunds a payment with the specified transaction ID.
        /// </summary>
        /// <param name="transactionId">The transaction ID of the payment to refund.</param>
        void Refund(Guid transactionId);
    }
}
