namespace GoFPatterns.State.OrderLifecycle.States
{
    // Base state - by default every action is not allowed.
    // Concrete states override only actions which make sense for them.
    public abstract class OrderStateBase : IOrderState
    {
        public abstract string Name { get; }

        public virtual void Pay(Order order) => Deny(nameof(Pay));
        public virtual void Ship(Order order) => Deny(nameof(Ship));
        public virtual void Deliver(Order order) => Deny(nameof(Deliver));
        public virtual void Cancel(Order order) => Deny(nameof(Cancel));

        private void Deny(string action) =>
            Console.WriteLine($"    Can't {action.ToLower()}: order is {Name}");
    }
}
