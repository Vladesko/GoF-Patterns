namespace GoFPatterns.State.OrderLifecycle
{
    // Состояние - объявляет все действия, которые можно выполнить с заказом.
    // Каждое конкретное состояние само решает, что означает действие в этом состоянии.
    public interface IOrderState
    {
        string Name { get; }
        void Pay(Order order);
        void Ship(Order order);
        void Deliver(Order order);
        void Cancel(Order order);
    }
}
