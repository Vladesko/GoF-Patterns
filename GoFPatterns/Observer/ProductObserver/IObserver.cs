namespace GoFPatterns.Observer.ProductObserver
{
    // Diese Schnittstelle wird von Klassen implementiert, die über Änderungen am Subjekt benachrichtigt werden wollen.
    public interface IObserver
    {
        void Update(decimal price);
    }
}
