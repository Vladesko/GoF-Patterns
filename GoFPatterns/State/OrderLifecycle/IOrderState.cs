namespace GoFPatterns.State.OrderLifecycle
{
    // State - declares all actions which can be done with an order.
    // Every concrete state decides what the action means in this state.
    public interface IOrderState
    {
        string Name { get; }
        void Pay(Order order);
        void Ship(Order order);
        void Deliver(Order order);
        void Cancel(Order order);
    }
}
