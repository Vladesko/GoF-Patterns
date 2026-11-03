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

            product.ChangePrice(120); //Non succede nulla
            product.ChangePrice(90); //Il Buyer fa qualcosa
            product.ChangePrice(40); //Il Notifier fa qualcosa
        }
    }
}
