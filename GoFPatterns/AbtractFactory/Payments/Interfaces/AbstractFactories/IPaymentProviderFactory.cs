using GoFPatterns.AbtractFactory.Payments.Interfaces.Abstractions;

namespace GoFPatterns.AbtractFactory.Payments.Interfaces.AbstractFactories
{
    public interface IPaymentProviderFactory
    {
        IPaymentProcessor CreatePaymentProcess();
        IRefundProcessor CreateRefundProcess();
        IPaymentValidator CreatePaymentValidator();
    }
}
