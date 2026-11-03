namespace GoFPatterns.State.OrderLifecycle.States
{
    // Estado base: por defecto ninguna acción está permitida.
    // Los estados concretos sobrescriben solo las acciones que tienen sentido para ellos.
    public abstract class OrderStateBase : IOrderState
    {
        public abstract string Name { get; }

        public virtual void Pay(Order order) => Deny("pagar");
        public virtual void Ship(Order order) => Deny("enviar");
        public virtual void Deliver(Order order) => Deny("entregar");
        public virtual void Cancel(Order order) => Deny("cancelar");

        private void Deny(string action) =>
            Console.WriteLine($"    No se puede {action}: el pedido está en estado {Name}");
    }
}
