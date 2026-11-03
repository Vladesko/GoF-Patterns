namespace GoFPatterns.Observer.ProductObserver
{
    // Questa è l'interfaccia implementata dalle classi che vogliono essere notificate delle modifiche del soggetto.
    public interface IObserver
    {
        void Update(decimal price);
    }
}
