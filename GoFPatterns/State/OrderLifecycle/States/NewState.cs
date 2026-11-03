namespace GoFPatterns.State.OrderLifecycle.States
{
    // Konkreter Zustand – die Bestellung ist angelegt, aber noch nicht bezahlt.
    public class NewState : OrderStateBase
    {
        public override string Name => "Neu";

        public override void Pay(Order order)
        {
            Console.WriteLine($"    Zahlung über {order.Amount:F2} angenommen");
            order.TransitionTo(new PaidState());
        }

        public override void Cancel(Order order) => order.TransitionTo(new CancelledState());
    }
}
