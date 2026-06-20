using GoFPatterns.Observer.ProductObserver.Observers;

namespace GoFPatterns.Observer.ProductObserver
{
    internal class ProductObserverDemonstrate
    {
        public void Demonstrate()
        {
            var product = new Product(150);
            var statistician = new StatisticsCollector(product);
            var notifier = new DiscountNotifier(product);
            var buyer = new Buyer(product);

            product.ChangePrice(120); //Nothing happens
            product.ChangePrice(90); //Buyer doing something
            product.ChangePrice(40); //Notifier doing something
        }
    }
}
