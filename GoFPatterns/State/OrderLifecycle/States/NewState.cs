namespace GoFPatterns.State.OrderLifecycle.States
{
    // Concrete state - order is created but not paid yet.
    public class NewState : OrderStateBase
    {
        public override string Name => "New";

        public override void Pay(Order order)
        {
            Console.WriteLine($"    Payment of {order.Amount:F2} accepted");
            order.TransitionTo(new PaidState());
        }

        public override void Cancel(Order order) => order.TransitionTo(new CancelledState());
    }
}
