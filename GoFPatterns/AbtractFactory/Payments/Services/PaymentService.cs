using GoFPatterns.AbtractFactory.Payments.Interfaces.AbstractFactories;
using GoFPatterns.AbtractFactory.Payments.Interfaces.Abstractions;

namespace GoFPatterns.AbtractFactory.Payments.Services
{
    internal class PaymentService(IPaymentProviderFactory paymentProviderFactory) : IPaymentService
    {
        private readonly IPaymentProcessor _paymentProcessor = paymentProviderFactory.CreatePaymentProcess();
        private readonly IRefundProcessor _refundProcessor = paymentProviderFactory.CreateRefundProcess();
        private readonly IPaymentValidator _paymentValidator = paymentProviderFactory.CreatePaymentValidator();

        public Guid Pay(decimal amount)
        {
            var isValid = _paymentValidator.ValidatePayment(amount);
            if (!isValid) 
            {
                throw new InvalidOperationException("Ungültiger Zahlungsbetrag.");
            }
            return _paymentProcessor.ProcessPayment(amount);
        }

        public void Refund(Guid transactionId)
        {
            _refundProcessor.ProcessRefund(transactionId);
        }
    }
}
