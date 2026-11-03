namespace GoFPatterns.Observer.ProductObserver
{
    // Schnittstelle des Subjekts: Über sie melden sich Beobachter an und ab, um über Änderungen benachrichtigt zu werden.
    public interface IObservable
    {
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);
    }
}
