namespace GoFPatterns.State.OrderLifecycle.States
{
    // Concrete state - order is on the way. It can't be cancelled anymore.
    public class ShippedState : OrderStateBase
    {
        public override string Name => "Shipped";

        public override void Deliver(Order order)
        {
            Console.WriteLine("    Customer received the package");
            order.TransitionTo(new DeliveredState());
        }
    }
}
