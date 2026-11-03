namespace GoFPatterns.AbtractFactory.Payments.Interfaces.Abstractions
{
    /// <summary>
    /// Il servizio di pagamento semplice
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>
        /// Elabora un pagamento dell'importo specificato.
        /// </summary>
        /// <param name="amount">L'importo da pagare.</param>
        Guid Pay(decimal amount);

        /// <summary>
        /// Rimborsa un pagamento con l'ID di transazione specificato.
        /// </summary>
        /// <param name="transactionId">L'ID di transazione del pagamento da rimborsare.</param>
        void Refund(Guid transactionId);
    }
}
