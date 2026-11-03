namespace GoFPatterns.State.OrderLifecycle.States
{
    // Stato concreto - l'ordine è stato creato ma non ancora pagato.
    public class NewState : OrderStateBase
    {
        public override string Name => "Nuovo";

        public override void Pay(Order order)
        {
            Console.WriteLine($"    Pagamento di {order.Amount:F2} accettato");
            order.TransitionTo(new PaidState());
        }

        public override void Cancel(Order order) => order.TransitionTo(new CancelledState());
    }
}
