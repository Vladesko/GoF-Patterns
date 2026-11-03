namespace GoFPatterns.AbtractFactory.Payments.Interfaces.Abstractions
{
    public interface IPaymentProcessor
    {
        /// <summary>
        /// Gibt eine eindeutige Transaktionskennung für die verarbeitete Zahlung zurück.
        /// </summary>
        /// <param name="amount">Der zu verarbeitende Betrag.</param>
        /// <returns>Die eindeutige Transaktionskennung.</returns>
        Guid ProcessPayment(decimal amount);
    }
}
