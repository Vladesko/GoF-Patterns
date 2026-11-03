using GoFPatterns.State.OrderLifecycle.States;

namespace GoFPatterns.State.OrderLifecycle
{
    // Context - l'ordine. Non contiene da nessuna parte uno "switch (status)",
    // delega semplicemente ogni azione all'oggetto dello stato corrente.
    public class Order(int id, decimal amount)
    {
        public int Id { get; } = id;
        public decimal Amount { get; } = amount;
        public IOrderState State { get; private set; } = new NewState();

        // Gli stati chiamano questo metodo per portare l'ordine allo stato successivo.
        internal void TransitionTo(IOrderState state)
        {
            Console.WriteLine($"    Ordine #{Id}: {State.Name} -> {state.Name}");
            State = state;
        }

        public void Pay() => State.Pay(this);
        public void Ship() => State.Ship(this);
        public void Deliver() => State.Deliver(this);
        public void Cancel() => State.Cancel(this);
    }
}
