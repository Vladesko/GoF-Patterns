namespace GoFPatterns.AbtractFactory.Payments.Interfaces.Abstractions
{
    public interface IPaymentProcessor
    {
        /// <summary>
        /// Returns a unique transaction identifier for the processed payment.
        /// </summary>
        /// <param name="amount">The amount to be processed.</param>
        /// <returns>The unique transaction identifier.</returns>
        Guid ProcessPayment(decimal amount);
    }
}
