using GoFPatterns.AbtractFactory.Payments.Interfaces.AbstractFactories;
using GoFPatterns.AbtractFactory.Payments.Interfaces.Abstractions;

namespace GoFPatterns.AbtractFactory.Payments.Stopes
{
    internal class StropePaymentFactory : IPaymentProviderFactory
    {
        public IPaymentProcessor CreatePaymentProcess()
        {
            return new StropePaymentProcessor();
        }

        public IRefundProcessor CreateRefundProcess()
        {
            return new StropeRefundProcessor();
        }

        public IPaymentValidator CreatePaymentValidator()
        {
            return new StropePaymentValidator();
        }
    }
}
