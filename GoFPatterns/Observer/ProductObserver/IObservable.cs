namespace GoFPatterns.Observer.ProductObserver
{
    // Esta es la interfaz que implementan las clases cuyos cambios se notifican a los observadores.
    public interface IObservable
    {
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);
    }
}
