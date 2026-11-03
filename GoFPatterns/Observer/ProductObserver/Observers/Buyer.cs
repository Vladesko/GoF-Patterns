namespace GoFPatterns.Observer.ProductObserver.Observers
{
    public class Buyer : IObserver
    {
        private readonly IObservable _observable;
        public Buyer(IObservable observable)
        {
            _observable = observable;
            _observable.Subscribe(this);
        }

        public void Update(decimal price)
        {
            if (price > 50)
                return;

            Console.WriteLine("¡Comprando el producto!");
            _observable.Unsubscribe(this);
        }
    }
}
