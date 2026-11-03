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

            product.ChangePrice(120); // Ничего не происходит
            product.ChangePrice(90); // Покупатель что-то делает
            product.ChangePrice(40); // Уведомитель о скидках что-то делает
        }
    }
}
