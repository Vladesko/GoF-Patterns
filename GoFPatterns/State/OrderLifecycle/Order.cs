using GoFPatterns.State.OrderLifecycle.States;

namespace GoFPatterns.State.OrderLifecycle
{
    // Kontext – die Bestellung. Sie enthält nirgends ein "switch (status)",
    // sondern delegiert jede Aktion einfach an das aktuelle Zustandsobjekt.
    public class Order(int id, decimal amount)
    {
        public int Id { get; } = id;
        public decimal Amount { get; } = amount;
        public IOrderState State { get; private set; } = new NewState();

        // Die Zustände rufen diese Methode auf, um die Bestellung in den nächsten Zustand zu versetzen.
        internal void TransitionTo(IOrderState state)
        {
            Console.WriteLine($"    Bestellung Nr. {Id}: {State.Name} -> {state.Name}");
            State = state;
        }

        public void Pay() => State.Pay(this);
        public void Ship() => State.Ship(this);
        public void Deliver() => State.Deliver(this);
        public void Cancel() => State.Cancel(this);
    }
}
