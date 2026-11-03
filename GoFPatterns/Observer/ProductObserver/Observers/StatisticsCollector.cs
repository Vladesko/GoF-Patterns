namespace GoFPatterns.Observer.ProductObserver.Observers
{
    public class StatisticsCollector : IObserver
    {
        private readonly IObservable _observable;
        private int _count;
        public StatisticsCollector(IObservable observable)
        {
            _observable = observable;
            _observable.Subscribe(this);
        }
        public void Update(decimal price)
        {
            _count++;
            Console.WriteLine($"Цена обновлена {_count} раз(а), новая цена: {price}");
        }
    }
}
