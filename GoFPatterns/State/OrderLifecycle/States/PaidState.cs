namespace GoFPatterns.State.OrderLifecycle.States
{
    // Concrete state - order is paid. Cancel here means refund.
    public class PaidState : OrderStateBase
    {
        public override string Name => "Paid";

        public override void Ship(Order order)
        {
            Console.WriteLine("    Package is handed over to courier");
            order.TransitionTo(new ShippedState());
        }

        public override void Cancel(Order order)
        {
            Console.WriteLine($"    Refund {order.Amount:F2} to customer");
            order.TransitionTo(new CancelledState());
        }
    }
}
