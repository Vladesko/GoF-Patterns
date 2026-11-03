namespace GoFPatterns.State.OrderLifecycle.States
{
    // Konkreter Zustand – Endzustand.
    public class CancelledState : OrderStateBase
    {
        public override string Name => "Storniert";
    }
}
