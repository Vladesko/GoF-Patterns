namespace GoFPatterns.State.OrderLifecycle.States
{
    // Estado concreto: el pedido está creado pero todavía no está pagado.
    public class NewState : OrderStateBase
    {
        public override string Name => "Nuevo";

        public override void Pay(Order order)
        {
            Console.WriteLine($"    Pago de {order.Amount:F2} aceptado");
            order.TransitionTo(new PaidState());
        }

        public override void Cancel(Order order) => order.TransitionTo(new CancelledState());
    }
}
