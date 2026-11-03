namespace GoFPatterns.State.OrderLifecycle.States
{
    // Stato concreto - l'ordine è pagato. Qui annullare significa rimborsare.
    public class PaidState : OrderStateBase
    {
        public override string Name => "Pagato";

        public override void Ship(Order order)
        {
            Console.WriteLine("    Il pacco è stato consegnato al corriere");
            order.TransitionTo(new ShippedState());
        }

        public override void Cancel(Order order)
        {
            Console.WriteLine($"    Rimborso di {order.Amount:F2} al cliente");
            order.TransitionTo(new CancelledState());
        }
    }
}
