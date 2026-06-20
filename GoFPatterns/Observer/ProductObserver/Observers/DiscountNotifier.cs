namespace GoFPatterns.Observer.ProductObserver.Observers
{
    public class DiscountNotifier : IObserver
    {
        private readonly IObservable _observable;
        public DiscountNotifier(IObservable observable)
        {
            _observable = observable;
            _observable.Subscribe(this);
        }
        public void Update(decimal price)
        {
            if (price > 100)
                return;

            Console.WriteLine("Discount applied!");
        }
    }
}
