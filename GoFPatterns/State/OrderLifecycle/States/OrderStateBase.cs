namespace GoFPatterns.State.OrderLifecycle.States
{
    // Basiszustand – standardmäßig ist keine Aktion erlaubt.
    // Konkrete Zustände überschreiben nur die Aktionen, die für sie sinnvoll sind.
    public abstract class OrderStateBase : IOrderState
    {
        public abstract string Name { get; }

        public virtual void Pay(Order order) => Deny("Bezahlen");
        public virtual void Ship(Order order) => Deny("Versenden");
        public virtual void Deliver(Order order) => Deny("Zustellen");
        public virtual void Cancel(Order order) => Deny("Stornieren");

        private void Deny(string action) =>
            Console.WriteLine($"    {action} nicht möglich: Bestellstatus ist {Name}");
    }
}
