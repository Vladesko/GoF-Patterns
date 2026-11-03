using GoFPatterns.AbtractFactory.Payments.Interfaces.Abstractions;
using GoFPatterns.AbtractFactory.Payments.PoolPays;
using GoFPatterns.AbtractFactory.Payments.Services;
using GoFPatterns.AbtractFactory.Payments.Stopes;

namespace GoFPatterns.AbtractFactory.Payments
{
    public class DemontrateAbstractFactory
    {
        public void Demonstrate()
        {

            Console.WriteLine("\tUso di PoolPay per le transazioni:\n");

            var poolPayPaymentProviderFactory = new PoolPayPaymentFactory();

            IPaymentService paymentServicePoolPay = new PaymentService(poolPayPaymentProviderFactory);

            var poolPayTransactionId = paymentServicePoolPay.Pay(100.00m);

            paymentServicePoolPay.Refund(poolPayTransactionId);

            Console.WriteLine("\n\tUso di Strope per le transazioni:\n");

            var stropePaymentProviderFactory = new StropePaymentFactory();

            IPaymentService paymentServiceStrope = new PaymentService(stropePaymentProviderFactory);

            var stropeTransactionId = paymentServiceStrope.Pay(200.00m);

            paymentServiceStrope.Refund(stropeTransactionId);
        }
    }
}
