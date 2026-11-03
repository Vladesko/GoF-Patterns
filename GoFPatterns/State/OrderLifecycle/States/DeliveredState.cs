namespace GoFPatterns.State.OrderLifecycle.States
{
    // Konkreter Zustand – Endzustand. Alle Aktionen werden von der Basisklasse abgelehnt.
    public class DeliveredState : OrderStateBase
    {
        public override string Name => "Zugestellt";
    }
}
