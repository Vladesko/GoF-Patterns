namespace GoFPatterns.AbtractFactory.Payments.Interfaces.Abstractions
{
    public interface IPaymentValidator
    {
        bool ValidatePayment(decimal amount);
    }
}
