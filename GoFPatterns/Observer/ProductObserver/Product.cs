namespace GoFPatterns.Observer.ProductObserver
{
    public class Product(decimal initialPrice) : IObservable
    {
        private readonly HashSet<IObserver> _observers = [];
        private decimal _price = initialPrice;

        public decimal Price => _price;

        public void ChangePrice(decimal newPrice)
        {
            if (_price == newPrice)
                return;

            _price = newPrice;
            NotifyObservers();
        }

        public void Subscribe(IObserver observer)
        {
            ArgumentNullException.ThrowIfNull(observer);
            _observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            ArgumentNullException.ThrowIfNull(observer);
            _observers.Remove(observer);
        }

        // Notify all observers about the price change.
        // This method also need for implementation of the IObservable interface.
        // But it is private because it is not intended to be called from outside the class.
        private void NotifyObservers()
        {
            foreach (var observer in _observers.ToArray())
            {
                observer.Update(_price);
            }
        }
    }
}
