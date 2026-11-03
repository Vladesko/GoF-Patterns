namespace GoFPatterns.State.OrderLifecycle.States
{
    // Concrete state - final state. All actions are denied by base class.
    public class DeliveredState : OrderStateBase
    {
        public override string Name => "Delivered";
    }
}
