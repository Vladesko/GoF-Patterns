namespace GoFPatterns.AbtractFactory.Payments.Interfaces.Abstractions
{
    /// <summary>
    /// Servicio de pagos sencillo
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>
        /// Procesa un pago por el importe indicado.
        /// </summary>
        /// <param name="amount">El importe a pagar.</param>
        Guid Pay(decimal amount);

        /// <summary>
        /// Reembolsa el pago con el identificador de transacción indicado.
        /// </summary>
        /// <param name="transactionId">El identificador de transacción del pago a reembolsar.</param>
        void Refund(Guid transactionId);
    }
}
