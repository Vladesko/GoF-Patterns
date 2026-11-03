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

            product.ChangePrice(120); // Nichts passiert
            product.ChangePrice(90); // Der Rabatt-Benachrichtiger reagiert (Preis <= 100)
            product.ChangePrice(40); // Der Käufer reagiert (Preis <= 50)
        }
    }
}
