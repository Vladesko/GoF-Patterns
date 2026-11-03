namespace GoFPatterns.State.OrderLifecycle.States
{
    // Stato concreto - l'ordine è in viaggio. Non può più essere annullato.
    public class ShippedState : OrderStateBase
    {
        public override string Name => "Spedito";

        public override void Deliver(Order order)
        {
            Console.WriteLine("    Il cliente ha ricevuto il pacco");
            order.TransitionTo(new DeliveredState());
        }
    }
}
