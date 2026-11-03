namespace GoFPatterns.State.OrderLifecycle
{
    // Estado: declara todas las acciones que se pueden realizar con un pedido.
    // Cada estado concreto decide qué significa la acción en ese estado.
    public interface IOrderState
    {
        string Name { get; }
        void Pay(Order order);
        void Ship(Order order);
        void Deliver(Order order);
        void Cancel(Order order);
    }
}
