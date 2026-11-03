using GoFPatterns.State.OrderLifecycle.States;

namespace GoFPatterns.State.OrderLifecycle
{
    // Context - order. It doesn't contain "switch (status)" anywhere,
    // it just delegates every action to the current state object.
    public class Order(int id, decimal amount)
    {
        public int Id { get; } = id;
        public decimal Amount { get; } = amount;
        public IOrderState State { get; private set; } = new NewState();

        // States call this method to switch the order to the next state.
        internal void TransitionTo(IOrderState state)
        {
            Console.WriteLine($"    Order #{Id}: {State.Name} -> {state.Name}");
            State = state;
        }

        public void Pay() => State.Pay(this);
        public void Ship() => State.Ship(this);
        public void Deliver() => State.Deliver(this);
        public void Cancel() => State.Cancel(this);
    }
}
