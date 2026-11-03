namespace GoFPatterns.State.OrderLifecycle.States
{
    // Estado concreto: el pedido está pagado. Aquí cancelar significa reembolsar.
    public class PaidState : OrderStateBase
    {
        public override string Name => "Pagado";

        public override void Ship(Order order)
        {
            Console.WriteLine("    El paquete se entrega al mensajero");
            order.TransitionTo(new ShippedState());
        }

        public override void Cancel(Order order)
        {
            Console.WriteLine($"    Reembolso de {order.Amount:F2} al cliente");
            order.TransitionTo(new CancelledState());
        }
    }
}
