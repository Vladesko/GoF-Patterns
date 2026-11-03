namespace GoFPatterns.State.OrderLifecycle
{
    // Zustand – deklariert alle Aktionen, die mit einer Bestellung möglich sind.
    // Jeder konkrete Zustand entscheidet, was die Aktion in diesem Zustand bedeutet.
    public interface IOrderState
    {
        string Name { get; }
        void Pay(Order order);
        void Ship(Order order);
        void Deliver(Order order);
        void Cancel(Order order);
    }
}
