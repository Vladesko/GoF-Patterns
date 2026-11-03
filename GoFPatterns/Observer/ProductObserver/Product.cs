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

        // Benachrichtigt alle Beobachter über die Preisänderung.
        // Diese Methode gehört ebenfalls zur Umsetzung der Schnittstelle IObservable.
        // Sie ist jedoch privat, weil sie nicht von außerhalb der Klasse aufgerufen werden soll.
        private void NotifyObservers()
        {
            foreach (var observer in _observers.ToArray())
            {
                observer.Update(_price);
            }
        }
    }
}
