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

        // Notifica a todos los observadores el cambio de precio.
        // Este método también es necesario para implementar la interfaz IObservable.
        // Pero es privado porque no está pensado para llamarse desde fuera de la clase.
        private void NotifyObservers()
        {
            foreach (var observer in _observers.ToArray())
            {
                observer.Update(_price);
            }
        }
    }
}
