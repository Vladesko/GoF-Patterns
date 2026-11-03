namespace GoFPatterns.AbtractFactory.Payments.Interfaces.Abstractions
{
    /// <summary>
    /// Простой платёжный сервис
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>
        /// Обрабатывает платёж на указанную сумму.
        /// </summary>
        /// <param name="amount">Сумма к оплате.</param>
        Guid Pay(decimal amount);

        /// <summary>
        /// Возвращает средства по платежу с указанным идентификатором транзакции.
        /// </summary>
        /// <param name="transactionId">Идентификатор транзакции платежа, по которому нужен возврат.</param>
        void Refund(Guid transactionId);
    }
}
