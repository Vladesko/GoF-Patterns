using GoFPatterns.AbtractFactory.Payments.Interfaces.AbstractFactories;
using GoFPatterns.AbtractFactory.Payments.Interfaces.Abstractions;

namespace GoFPatterns.AbtractFactory.Payments.PoolPays
{
    internal class PoolPayPaymentFactory : IPaymentProviderFactory
    {
        public IPaymentProcessor CreatePaymentProcess()
        {
            return new PoolPayPaymentProcess();
        }

        public IPaymentValidator CreatePaymentValidator()
        {
             return new PoolPayPaymentValidator();
        }

        public IRefundProcessor CreateRefundProcess()
        {
            return new PoolPayRefundProcessor();
        }
    }
}
