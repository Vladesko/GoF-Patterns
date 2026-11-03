namespace GoFPatterns.AbtractFactory.Payments.Interfaces.Abstractions
{
    /// <summary>
    /// Ein einfacher Zahlungsdienst
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>
        /// Verarbeitet eine Zahlung über den angegebenen Betrag.
        /// </summary>
        /// <param name="amount">Der zu zahlende Betrag.</param>
        Guid Pay(decimal amount);

        /// <summary>
        /// Erstattet eine Zahlung mit der angegebenen Transaktions-ID.
        /// </summary>
        /// <param name="transactionId">Die Transaktions-ID der zu erstattenden Zahlung.</param>
        void Refund(Guid transactionId);
    }
}
