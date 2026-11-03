namespace GoFPatterns.State.OrderLifecycle
{
    // State - dichiara tutte le azioni che si possono eseguire su un ordine.
    // Ogni stato concreto decide cosa significa l'azione in quello stato.
    public interface IOrderState
    {
        string Name { get; }
        void Pay(Order order);
        void Ship(Order order);
        void Deliver(Order order);
        void Cancel(Order order);
    }
}
