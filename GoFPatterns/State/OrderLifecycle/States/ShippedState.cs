namespace GoFPatterns.State.OrderLifecycle.States
{
    // Konkreter Zustand – die Bestellung ist unterwegs. Sie kann nicht mehr storniert werden.
    public class ShippedState : OrderStateBase
    {
        public override string Name => "Versandt";

        public override void Deliver(Order order)
        {
            Console.WriteLine("    Der Kunde hat das Paket erhalten");
            order.TransitionTo(new DeliveredState());
        }
    }
}
