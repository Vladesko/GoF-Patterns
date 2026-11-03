namespace GoFPatterns.State.OrderLifecycle.States
{
    // Konkreter Zustand – die Bestellung ist bezahlt. Stornieren bedeutet hier Erstattung.
    public class PaidState : OrderStateBase
    {
        public override string Name => "Bezahlt";

        public override void Ship(Order order)
        {
            Console.WriteLine("    Paket wurde dem Kurier übergeben");
            order.TransitionTo(new ShippedState());
        }

        public override void Cancel(Order order)
        {
            Console.WriteLine($"    Erstattung von {order.Amount:F2} an den Kunden");
            order.TransitionTo(new CancelledState());
        }
    }
}
