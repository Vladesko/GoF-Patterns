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
            Console.WriteLine($"Precio actualizado {_count} veces a {price}");
        }
    }
}
