namespace GoFPatterns.State.OrderLifecycle.States
{
    // Stato concreto - stato finale. Tutte le azioni vengono negate dalla classe base.
    public class DeliveredState : OrderStateBase
    {
        public override string Name => "Consegnato";
    }
}
