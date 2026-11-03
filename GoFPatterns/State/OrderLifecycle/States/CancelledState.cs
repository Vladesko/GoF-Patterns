namespace GoFPatterns.State.OrderLifecycle.States
{
    // Stato concreto - stato finale.
    public class CancelledState : OrderStateBase
    {
        public override string Name => "Annullato";
    }
}
