namespace GoFPatterns.Observer.ProductObserver
{
    //Questa è l'interfaccia implementata dalle classi che vogliono essere notificate delle modifiche del soggetto.
    public interface IObservable
    {
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);
    }
}
