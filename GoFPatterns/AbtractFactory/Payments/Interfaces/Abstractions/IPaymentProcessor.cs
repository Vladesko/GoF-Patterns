namespace GoFPatterns.AbtractFactory.Payments.Interfaces.Abstractions
{
    public interface IPaymentProcessor
    {
        /// <summary>
        /// Возвращает уникальный идентификатор транзакции для обработанного платежа.
        /// </summary>
        /// <param name="amount">Сумма к обработке.</param>
        /// <returns>Уникальный идентификатор транзакции.</returns>
        Guid ProcessPayment(decimal amount);
    }
}
