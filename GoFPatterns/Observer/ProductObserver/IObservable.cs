namespace GoFPatterns.Observer.ProductObserver
{
    // Это интерфейс, который реализуют классы, за изменениями которых наблюдают (субъекты).
    public interface IObservable
    {
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);
    }
}
