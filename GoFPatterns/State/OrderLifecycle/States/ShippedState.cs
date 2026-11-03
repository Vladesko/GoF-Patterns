namespace GoFPatterns.State.OrderLifecycle.States
{
    // Estado concreto: el pedido está en camino. Ya no se puede cancelar.
    public class ShippedState : OrderStateBase
    {
        public override string Name => "Enviado";

        public override void Deliver(Order order)
        {
            Console.WriteLine("    El cliente recibió el paquete");
            order.TransitionTo(new DeliveredState());
        }
    }
}
