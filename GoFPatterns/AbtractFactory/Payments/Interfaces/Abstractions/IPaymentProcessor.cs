namespace GoFPatterns.AbtractFactory.Payments.Interfaces.Abstractions
{
    public interface IPaymentProcessor
    {
        /// <summary>
        /// Devuelve un identificador único de transacción para el pago procesado.
        /// </summary>
        /// <param name="amount">El importe que se va a procesar.</param>
        /// <returns>El identificador único de la transacción.</returns>
        Guid ProcessPayment(decimal amount);
    }
}
