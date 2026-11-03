namespace GoFPatterns.Observer.ProductObserver
{
    // This is the interface that will be implemented by the classes that want to be notified of changes in the subject.
    public interface IObserver
    {
        void Update(decimal price);
    }
}
