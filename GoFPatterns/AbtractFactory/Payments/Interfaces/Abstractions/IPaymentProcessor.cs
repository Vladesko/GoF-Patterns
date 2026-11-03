namespace GoFPatterns.AbtractFactory.Payments.Interfaces.Abstractions
{
    public interface IPaymentProcessor
    {
        /// <summary>
        /// Restituisce un identificatore univoco della transazione per il pagamento elaborato.
        /// </summary>
        /// <param name="amount">L'importo da elaborare.</param>
        /// <returns>L'identificatore univoco della transazione.</returns>
        Guid ProcessPayment(decimal amount);
    }
}
