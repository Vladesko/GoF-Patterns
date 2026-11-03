namespace GoFPatterns.State.OrderLifecycle.States
{
    // Concrete state - final state.
    public class CancelledState : OrderStateBase
    {
        public override string Name => "Cancelled";
    }
}
